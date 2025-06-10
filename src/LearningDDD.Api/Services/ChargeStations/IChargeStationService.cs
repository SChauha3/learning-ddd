using LearningDDD.Api.Dtos.ChargeStation;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Services.ChargeStations
{
    public interface IChargeStationService
    {
        Task<Result<ChargeStation>> CreateChargeStationAsync(CreateChargeStation createChargeStation);
        Task<Result<bool>> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation);
        Task<Result<bool>> DeleteChargeStationAsync(Guid id, Guid groupId);
    }
}