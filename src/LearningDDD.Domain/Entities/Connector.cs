using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Domain.Models
{
    public class Connector : Entity<Guid>
    {
        public int ChargeStationContextId { get; private set; }
        public int MaxCurrent { get; private set; }

        private Connector(Guid id, int chargeStationContextId, int maxCurrent) : base(id)
        {
            ChargeStationContextId = chargeStationContextId;
            MaxCurrent = maxCurrent;
        }

        internal static Connector Create(int chargeStationContextId, int maxCurrent) =>
            new Connector(Guid.Empty, chargeStationContextId, maxCurrent);

        internal void Update(int newMaxCurrent) =>
            MaxCurrent = newMaxCurrent;
    }
}