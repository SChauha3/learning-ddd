using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartCharging.Api.Models
{
    public class ChargeStation
    {
        [Key]
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public ICollection<Connector> Connectors {  get; set; } = new List<Connector>();
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Group? Group { get; set; }
    }
}