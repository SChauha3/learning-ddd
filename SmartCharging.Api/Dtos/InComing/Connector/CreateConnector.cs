namespace SmartCharging.Api.Dtos.Connector
{
    public class CreateConnector
    {
        public int ChargeStationContextId { get; set; }
        public string ChargeStationId { get; set; }
        public int MaxCurrent { get; set; }
    }
}
