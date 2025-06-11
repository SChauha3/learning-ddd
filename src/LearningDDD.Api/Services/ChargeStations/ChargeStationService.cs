using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Domain.Interfaces;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace LearningDDD.Api.Services.ChargeStations
{
    public class ChargeStationService : IChargeStationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChargeStationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ChargeStation>> CreateChargeStationAsync(CreateChargeStation createChargeStation)
        {
            var isGroupIdValidGuid = Guid.TryParse(createChargeStation.GroupId, out var parsedGroupId);
            if (!isGroupIdValidGuid)
                return Result<ChargeStation>.Fail(
                    "The GroupId provided is not a valid GUID.", 
                    ErrorType.InvalidId);

            var group = await _unitOfWork.Groups.FindAsync(
                q => q.Id == parsedGroupId,
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<ChargeStation>.Fail(
                    "The specified group was not found, and charge station cannot be created without a valid group", 
                    ErrorType.GroupNotFound);

            var chargeStationCreationResult = group.AddChargeStation(
                createChargeStation.Name, 
                createChargeStation.Connectors.Select(c => (c.ChargeStationContextId, c.MaxCurrent)));

            if(chargeStationCreationResult.IsSuccess)
                await _unitOfWork.SaveChangesAsync();

            return chargeStationCreationResult;
        }

        public async Task<Result<bool>> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation)
        {
            var isGroupIdValidGuid = Guid.TryParse(updateChargeStation.GroupId, out var parsedGroupId);
            if (!isGroupIdValidGuid)
                return Result<bool>.Fail(
                    "The GroupId provided is not a valid GUID.",
                    ErrorType.InvalidId);

            var group = await _unitOfWork.Groups.FindAsync(
                q => q.Id == parsedGroupId,
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<bool>.Fail(
                    $"Group with id {updateChargeStation.GroupId} not found.", 
                    ErrorType.GroupNotFound);

            var result = group.UpdateChargeStation(id, updateChargeStation.Name);
            if(result.IsSuccess)
                await _unitOfWork.SaveChangesAsync();
                
            return result;
        }

        public async Task<Result<bool>> DeleteChargeStationAsync(Guid chargeStationId, Guid groupId)
        {
            var group = await _unitOfWork.Groups.FindAsync(
                q => q.Id == groupId,
                q => q.Include(g => g.ChargeStations).ThenInclude(cs => cs.Connectors));

            if (group is null)
                return Result<bool>.Fail(
                    $"Group with id {groupId} not found.",
                    ErrorType.GroupNotFound);

            var result = group.RemoveChargeStation(chargeStationId);

            if (!result.IsSuccess || result.Value is null)
                return Result<bool>.Fail(result.Error ?? "An unknown error occurred.", result.ErrorType ?? ErrorType.Unknown);

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
