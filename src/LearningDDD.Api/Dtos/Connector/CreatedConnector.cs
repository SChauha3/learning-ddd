using System.ComponentModel.DataAnnotations;

namespace LearningDDD.Api.Dtos.Connector
{
    public class CreatedConnector
    {
        public Guid Id { get; set; }
        public int ChargeStationContextId { get; set; }
        public int MaxCurrent { get; set; }
    }
}
