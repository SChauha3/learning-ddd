using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Api.Models
{
    public class Group
    {
        [Key]
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public ICollection<ChargeStation> ChargeStations { get; private set; } = new List<ChargeStation>();
    }
}