namespace SmartCharging.Api.Dtos.Outgoing
{
    public class CreatedChargeStation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CreatedConnector> Connectors { get; set; }
    }
}
