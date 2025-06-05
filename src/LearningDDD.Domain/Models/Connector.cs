namespace LearningDDD.Domain.Models
{
    public class Connector
    {
        public Guid Id { get; private set; }
        public int ChargeStationContextId { get; private set; }
        public int MaxCurrent { get; private set; }

        private Connector(int chargeStationContextId, int maxCurrent)
        {
            Id = Guid.NewGuid();
            ChargeStationContextId = chargeStationContextId;
            MaxCurrent = maxCurrent;
        }

        internal static Connector Create(int chargeStationContextId, int maxCurrent) => 
            new Connector(chargeStationContextId, maxCurrent);

        internal void UpdateMaxCurrent(int newMaxCurrent) =>
            MaxCurrent = newMaxCurrent;
    }
}