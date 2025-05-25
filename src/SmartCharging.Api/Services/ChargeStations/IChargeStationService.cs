using SmartCharging.Api.Dtos.ChargeStation;

namespace SmartCharging.Api.Services.ChargeStations
{
    public interface IChargeStationService
    {
        Task<Result<Guid>> CreateChargeStationAsync(CreateChargeStation createChargeStation);
        Task<Result> UpdateChargeStationAsync(Guid id, UpdateChargeStation updateChargeStation);
        Task<Result> DeleteChargeStationAsync(Guid id);
    }
}