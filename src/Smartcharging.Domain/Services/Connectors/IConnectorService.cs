using SmartCharging.Api.Dtos.Connector;

namespace SmartCharging.Api.Services.Connectors
{
    public interface IConnectorService
    {
        Task<Result<Guid>> CreateConnectorAsync(CreateConnector connectorDto);
        Task<Result> UpdateConnectorAsync(Guid id, UpdateConnector updateConnector);
        Task<Result> DeleteConnectorAsync(Guid id);
    }
}