using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Domain.Interfaces;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace LearningDDD.Api.Services.Connectors
{
    public class ConnectorService : IConnectorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConnectorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Connector>> CreateConnectorAsync(CreateConnector createConnector)
        {
            var isGroupIdValidGuid = Guid.TryParse(createConnector.GroupId, out var parsedGroupId);
            var isChargeStationIdValidGuid = Guid.TryParse(createConnector.ChargeStationId, out var parsedChargeStationId);
            if (!isGroupIdValidGuid || !isChargeStationIdValidGuid)
                return Result<Connector>.Fail(
                    "The provided GroupId or ChargeStationId is not a valid GUID.",
                    ErrorType.InvalidId);

            var group = await _unitOfWork.Groups.FindAsync(
                g => g.Id == parsedGroupId,
                cs => cs.Include(cs => cs.ChargeStations).ThenInclude(c => c.Connectors));

            if (group is null)
                return Result<Connector>.Fail(
                    "A Connector cannot exist in the domain without a Charge Station and group.", 
                    ErrorType.GroupNotFound);

            var result = group.AddConnectorToChargeStation(
                createConnector.ChargeStationContextId,
                createConnector.MaxCurrent,
                parsedChargeStationId);

            if (result.IsSuccess)
                await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Result<bool>> UpdateConnectorAsync(Guid connectorId, UpdateConnector updateConnector)
        {
            var isGroupIdValidGuid = Guid.TryParse(updateConnector.GroupId, out var parsedGroupId);
            var isChargeStationIdValidGuid = Guid.TryParse(updateConnector.ChargeStationId, out var parsedChargeStationId);
            if (!isGroupIdValidGuid || !isChargeStationIdValidGuid)
                return Result<bool>.Fail(
                    "The provided GroupId or ChargeStationId is not a valid GUID.",
                    ErrorType.InvalidId);

            var group = await _unitOfWork.Groups.FindAsync(
                g => g.Id == parsedGroupId,
                query => query
                    .Include(cs => cs.ChargeStations)
                    .ThenInclude(c => c.Connectors));

            if (group is null)
                return Result<bool>.Fail($"Group with id {connectorId} not found.", ErrorType.GroupNotFound);

            var result = group.UpdateConnector(
                updateConnector.MaxCurrent,
                parsedChargeStationId,
                connectorId);

            if (result.IsSuccess)
                await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Result<bool>> DeleteConnectorAsync(Guid connectorId, Guid chargeStationId, Guid groupId)
        {
            var group = await _unitOfWork.Groups.FindAsync(
                g => g.Id == groupId,
                query => query
                    .Include(cs => cs.ChargeStations)
                    .ThenInclude(c => c.Connectors));
            if (group is null)
                return Result<bool>.Fail($"Group with id {groupId} not found.", ErrorType.GroupNotFound);

            var result = group.RemoveConnector(connectorId, chargeStationId);
            if (!result.IsSuccess || result.Value is null)
                return Result<bool>.Fail(result.Error ?? "Unknown error occurred.", result.ErrorType ?? ErrorType.Unknown);

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}