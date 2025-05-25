using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Api.Dtos.Outgoing
{
    public class CreatedConnector
    {
        public Guid Id { get; set; }
        public int ChargeStationContextId { get; set; }
        public int MaxCurrent { get; set; }
    }
}
