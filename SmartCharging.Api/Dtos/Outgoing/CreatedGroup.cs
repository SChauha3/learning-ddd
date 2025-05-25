namespace SmartCharging.Api.Dtos.Outgoing
{
    public class CreatedGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<CreatedChargeStation> ChargeStations { get; set; }
    }
}
