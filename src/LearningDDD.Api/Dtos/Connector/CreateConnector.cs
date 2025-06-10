namespace LearningDDD.Api.Dtos.Connector
{
    public class CreateConnector
    {
        public string GroupId { get; set; }
        public string ChargeStationId { get; set; }
        public int ChargeStationContextId { get; set; }
        public int MaxCurrent { get; set; }
    }
}
