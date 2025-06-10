using LearningDDD.Api.Dtos.Connector;
using LearningDDD.Domain.Models;
using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Services.Connectors
{
    public interface IConnectorService
    {
        Task<Result<Connector>> CreateConnectorAsync(CreateConnector connectorDto);
        Task<Result<bool>> UpdateConnectorAsync(Guid id, UpdateConnector updateConnector);
        Task<Result<bool>> DeleteConnectorAsync(Guid id, Guid chargeStationId, Guid groupId);
    }
}