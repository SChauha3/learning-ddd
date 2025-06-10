using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.Ports;
using LearningDDD.Domain.SeedWork;
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

        public async Task<Result<ChargeStation>> CreateChargeStationAsync(CreateChargeStation createChargeStation)
        {
            var group = await _groupRepository.FindAsync(
                q => q.Id == Guid.Parse(createChargeStation.GroupId),
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<ChargeStation>.Fail(
                    "The specified group was not found, and charge station cannot be created without a valid group", 
                    ErrorType.GroupNotFound);

            var chargeStation = group.AddChargeStation(
                createChargeStation.Name, 
                createChargeStation.Connectors.Select(c => (c.ChargeStationContextId, c.MaxCurrent)));

            await _groupRepository.AddAsync(group);
            return chargeStation;
        }

        public async Task<Result<bool>> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation)
        {
            var group = await _groupRepository.FindByIdAsync(Guid.Parse(updateChargeStation.GroupId));
            if (group is null)
                return Result<bool>.Fail(
                    $"Group with id {updateChargeStation.GroupId} not found.", 
                    ErrorType.GroupNotFound);

            var result = group.UpdateChargeStation(id, updateChargeStation.Name);
            if(result.IsSuccess)
                await _groupRepository.UpdateAsync(group);

            return result;
        }

        public async Task<Result<bool>> DeleteChargeStationAsync(Guid id, Guid groupId)
        {
            var group = await _groupRepository.FindByIdAsync(groupId);
            if (group is null)
                return Result<bool>.Fail(
                    $"Group with id {groupId} not found.",
                    ErrorType.GroupNotFound);

            var result = group.RemoveChargeStation(id);

            if (!result.IsSuccess || result.Value is null)
            {
                // Ensure both `result.Error` and `result.ErrorType` are non-null before calling `Fail`.
                if (result.Error is not null && result.ErrorType.HasValue)
                {
                    return Result<bool>.Fail(result.Error, result.ErrorType.Value);
                }
                return Result<bool>.Fail("An unknown error occurred.", ErrorType.Unknown);
            }

            await _chargeStationRepository.DeleteAsync(result.Value);
            return Result<bool>.Success(true);
        }
    }
}
