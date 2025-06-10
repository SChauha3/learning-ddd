using Microsoft.EntityFrameworkCore;
using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Domain.Ports;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Services.Connectors
{
    public class ConnectorService : IConnectorService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Connector> _connectorRepository;

        public ConnectorService(IRepository<Group> groupRepository, IRepository<Connector> connectorRepository)
        {
            _connectorRepository = connectorRepository;
            _groupRepository = groupRepository;
        }

        public async Task<Result<Connector>> CreateConnectorAsync(CreateConnector createConnector)
        {
            var group = await _groupRepository.FindAsync(
                cs => cs.Id == Guid.Parse(createConnector.GroupId),
                cs => cs.Include(c => c.ChargeStations));

            if (group is null)
                return Result<Connector>.Fail(
                    "A Connector cannot exist in the domain without a Charge Station and group.", 
                    ErrorType.GroupNotFound);

            var result = group.AddConnectorToChargeStation(
                createConnector.ChargeStationContextId,
                createConnector.MaxCurrent,
                Guid.Parse(createConnector.ChargeStationId));

            await _groupRepository.AddAsync(group);
            return result;
        }

        public async Task<Result<bool>> UpdateConnectorAsync(Guid id, UpdateConnector updateConnector)
        {
            var group = await _groupRepository.FindAsync(
                c => c.Id == Guid.Parse(updateConnector.GroupId),
                query => query
                    .Include(c => c.ChargeStations)
                    .ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<bool>.Fail($"A Connector with id {id} does not belong to existing chargeStation.", ErrorType.GroupNotFound);

            var result = group.UpdateConnector(
                updateConnector.MaxCurrent,
                Guid.Parse(updateConnector.ChargeStationId),
                id);

            if (result.IsSuccess)
                await _groupRepository.UpdateAsync(group);

            return result;
        }

        public async Task<Result<bool>> DeleteConnectorAsync(Guid id, Guid chargeStationId, Guid groupId)
        {
            var group = await _groupRepository.FindAsync(
                c => c.Id == groupId,
                query => query
                    .Include(c => c.ChargeStations)
                    .ThenInclude(cs => cs.Connectors));
            if (group is null)
                return Result<bool>.Fail($"A Connector with id {id} does not belong to existing chargeStation.", ErrorType.GroupNotFound);

            var result = group.RemoveConnector(id, chargeStationId);
            if (!result.IsSuccess || result.Value is null)
                return Result<bool>.Fail(result.Error ?? "Unknown error occurred.", result.ErrorType ?? ErrorType.Unknown);

            await _connectorRepository.DeleteAsync(result.Value);
            return Result<bool>.Success(true);
        }
    }
}