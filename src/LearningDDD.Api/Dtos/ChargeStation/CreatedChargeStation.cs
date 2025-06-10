using LearningDDD.Api.Dtos.Connector;

namespace LearningDDD.Api.Dtos.ChargeStation
{
    public class CreatedChargeStation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CreatedConnector> Connectors { get; set; }
    }
}
