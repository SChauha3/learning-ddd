using LearningDDD.Api.Dtos.ChargeStation;

namespace LearningDDD.Api.Dtos.Group
{
    public class CreatedGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<CreatedChargeStation> ChargeStations { get; set; }
    }
}
