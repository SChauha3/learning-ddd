using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartCharging.Api.Models
{
    public class Connector
    {
        [Key]
        public Guid Id { get; private set; }
        public int ChargeStationContextId { get; private set; }
        public int MaxCurrent { get; private set; }
        public Guid ChargeStationId { get; private set; }
        [JsonIgnore]
        public ChargeStation? ChargeStation { get; private set; }
    }
}