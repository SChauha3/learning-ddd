using Microsoft.EntityFrameworkCore;
using SmartCharging.Api.Dtos.ChargeStation;
using SmartCharging.Api.Models;
using SmartCharging.Api.Repositories;

namespace SmartCharging.Api.Services.ChargeStations
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<ChargeStation> _chargeStationRepository;

        public ChargeStationService(
            IRepository<Group> groupRepository,
            IRepository<ChargeStation> chargeStationRepository)
        {
            _groupRepository = groupRepository;
            _chargeStationRepository = chargeStationRepository;
        }

        public async Task<Result<Guid>> CreateChargeStationAsync(CreateChargeStation createChargeStation)
        {
            var group = await _groupRepository.FindAsync(
                q => q.Id == Guid.Parse(createChargeStation.GroupId),
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            if (group == null)
                return Result<Guid>.Fail("The specified group was not found, and charge station cannot be created without a valid group", ErrorType.NotFound);

            var id = Guid.NewGuid();
            var connectors = MapConnectorsDtoToEntity(createChargeStation.Connectors, id);

            var chargeStation = ChargeStation.Create(id, createChargeStation.Name, connectors, group.Id);

            if (!group.CanAddChargeStation(chargeStation))
                return Result<Guid>.Fail("Group capacity exceeded.", ErrorType.InValidCapacity);

            group.ChargeStations.Add(chargeStation);
            await _chargeStationRepository.AddAsync(chargeStation);

            return Result<Guid>.Success(chargeStation.Id);
        }

        public async Task<Result> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation)
        {
            var chargeStation = await _chargeStationRepository.FindAsync(
                cs => cs.Id == id,
                q => q.Include(cs => cs.Connectors));

            if (chargeStation == null)
                return Result.Fail($"charge station with id {id} not found.", ErrorType.NotFound);

            chargeStation.Update(updateChargeStation.Name);
            
            await _chargeStationRepository.UpdateAsync(chargeStation);

            return Result.Success();
        }

        public async Task<Result> DeleteChargeStationAsync(Guid id)
        {
            var chargeStation = await _chargeStationRepository.FindByIdAsync(id);
            if (chargeStation == null)
                return Result.Fail("Charge station not found.", ErrorType.NotFound);

            await _chargeStationRepository.DeleteAsync(chargeStation);
            return Result.Success();
        }

        private ICollection<Connector> MapConnectorsDtoToEntity(List<CreateConnectorWithChargeStation> createConnectors, Guid id)
        {
            var connectors = new List<Connector>();
            foreach (var createConnector in createConnectors)
            {
                connectors.Add(
                    Connector.Create(
                        createConnector.ChargeStationContextId,
                        createConnector.MaxCurrent,
                        id));
            }

            return connectors;
        }
    }
}
