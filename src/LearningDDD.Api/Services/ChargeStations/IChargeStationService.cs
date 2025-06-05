using LearningDDD.Api.Dtos.ChargeStation;

namespace LearningDDD.Api.Services.ChargeStations
{
    public interface IChargeStationService
    {
        Task<Result<Guid>> CreateChargeStationAsync(CreateChargeStation createChargeStation);
        Task<Result> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation);
        Task<Result> DeleteChargeStationAsync(Guid id);
    }
}