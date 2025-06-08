using Microsoft.EntityFrameworkCore;
using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Domain.Ports;
using LearningDDD.Domain.Models;

namespace LearningDDD.Api.Services.Connectors
{
    public class ConnectorService : IConnectorService
    {
        private readonly IRepository<Group> _groupStationRepository;
        private readonly IRepository<ChargeStation> _chargeStationRepository;
        private readonly IRepository<Connector> _connectorRepository;

        public ConnectorService(
            IRepository<Group> groupStationRepository,
            IRepository<ChargeStation> chargeStationRepository,
            IRepository<Connector> connectorRepository)
        {
            _groupStationRepository = groupStationRepository;
            _chargeStationRepository = chargeStationRepository;
            _connectorRepository = connectorRepository;
        }

        public async Task<Result<Guid>> CreateConnectorAsync(CreateConnector createConnector)
        {
            var group = await _groupStationRepository.FindAsync(
                cs => cs.Id == Guid.Parse(createConnector.GroupId),
                cs => cs.Include(c => c.ChargeStations));

            if (group is null)
                return Result<Guid>.Fail("A Connector cannot exist in the domain without a Charge Station and group.", ErrorType.NotFound);

            if (!group.IsChargeStationContextIdUnique(createConnector.ChargeStationContextId, Guid.Parse(createConnector.ChargeStationId)))
                return Result<Guid>.Fail("Id must be unique within the context of a charge station with " +
                    "(possible range of values from 1 to 5)", ErrorType.UniqueConnector);

            if (!group.CanAddConnector(createConnector.MaxCurrent))
                return Result<Guid>.Fail("Total connector max current would exceed group's capacity.", ErrorType.UniqueConnector);

            var connectorId = group.AddConnectorToChargeStation(
                createConnector.ChargeStationContextId,
                createConnector.MaxCurrent,
                Guid.Parse(createConnector.ChargeStationId));

            await _groupStationRepository.AddAsync(group);
            return Result<Guid>.Success(connectorId);
        }

        public async Task<Result> UpdateConnectorAsync(Guid id, UpdateConnector updateConnector)
        {
            var connector = await _connectorRepository.FindAsync(
                c => c.Id == id,
                query => query
                    .Include(c => c.ChargeStation)
                    .ThenInclude(cs => cs.Group)
                    .ThenInclude(cs => cs.ChargeStations)
                    .ThenInclude(c => c.Connectors));

            if (connector is null)
                return Result.Fail($"A Connector with id {id} does not exist.", ErrorType.NotFound);

            if (!connector.CanUpdateMaxCurrent(updateConnector.MaxCurrent))
                return Result.Fail("Total connector max current would exceed group's capacity.", ErrorType.InValidCapacity);

            connector.UpdateMaxCurrent(updateConnector.MaxCurrent);

            await _connectorRepository.UpdateAsync(connector);
            return Result.Success();
        }

        public async Task<Result> DeleteConnectorAsync(Guid id)
        {
            var connector = await _connectorRepository.FindAsync(
                c => c.Id == id,
                query => query
                    .Include(c => c.ChargeStation)
                    .ThenInclude(cs => cs.Group)
                    .Include(c => c.ChargeStation.Connectors));

            if (connector is null)
                return Result.Fail($"Connector not found with id {id}.", ErrorType.NotFound);

            if (!connector.ChargeStation.CanRemoveConnector())
                return Result.Fail("At least one connector is required per charge station.", ErrorType.MinimumOneConnector);

            connector.ChargeStation.RemoveConnector(connector);

            await _connectorRepository.DeleteAsync(connector);
            return Result.Success();
        }
    }
}