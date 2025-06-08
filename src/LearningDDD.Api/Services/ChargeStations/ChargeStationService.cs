using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Api.Dtos.Outgoing;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace LearningDDD.Api.Services.ChargeStations
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

            if (group is null)
                return Result<Guid>.Fail("The specified group was not found, and charge station cannot be created without a valid group", ErrorType.NotFound);

            var chargeStationId = group.AddChargeStation(
                createChargeStation.Name, 
                createChargeStation.Connectors.Select(c => (c.ChargeStationContextId, c.MaxCurrent)));

            await _groupRepository.AddAsync(group);
            return Result<Guid>.Success(chargeStationId);
        }

        public async Task<Result> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation)
        {
            var group = await _groupRepository.FindByIdAsync(Guid.Parse(updateChargeStation.GroupId));
            if (group is null)
                return Result.Fail($"Group with id {updateChargeStation.GroupId} not found.", ErrorType.NotFound);

            group.UpdateChargeStation(id, updateChargeStation.Name);

            await _groupRepository.UpdateAsync(group);
            return Result.Success();
        }

        public async Task<Result> DeleteChargeStationAsync(Guid id)
        {
            var chargeStation = await _chargeStationRepository.FindByIdAsync(id);
            if (chargeStation is null)
                return Result.Fail("Charge station not found.", ErrorType.NotFound);

            await _chargeStationRepository.DeleteAsync(chargeStation);
            return Result.Success();
        }
    }
}
