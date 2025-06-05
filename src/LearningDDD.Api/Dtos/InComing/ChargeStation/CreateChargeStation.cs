namespace LearningDDD.Api.Dtos.ChargeStation
{
    public class CreateChargeStation
    {
        public string Name { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public List<CreateConnectorWithChargeStation> Connectors { get; set; } = new();
    }

    public class CreateConnectorWithChargeStation
    {
        public int ChargeStationContextId { get; set; }
        public int MaxCurrent { get; set; }
    }
}