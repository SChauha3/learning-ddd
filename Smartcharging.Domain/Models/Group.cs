namespace Smartcharging.Domain.Models
{
    public class Group
    {
        private readonly List<ChargeStation> _chargeStations = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }

        public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations.AsReadOnly();

        private Group(Guid id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }

        public static Group Create(string name, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Group name is required.");
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
            return new Group(Guid.NewGuid(), name, capacity);
        }

        public void AddChargeStation(string name, IEnumerable<(int chargeStationContextId, int maxCurrent)> connectors)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Charge station name is required.");

            if (connectors == null)
                throw new ArgumentNullException(nameof(connectors));

            var newConnectors = connectors.Select(c => Connector.Create(c.chargeStationContextId, c.maxCurrent)).ToList();

            ConnectorValidator.Validate(newConnectors);
            if (!CanAddConnectors(newConnectors))
                throw new InvalidOperationException("Adding this station would exceed the group's capacity.");

            var chargeStation = ChargeStation.Create(name, newConnectors);
            _chargeStations.Add(chargeStation);
        }

        public bool TryUpdate(string name, int newCapacity)
        {
            if (!CanUpdateCapacity(newCapacity))
                return false;

            Name = name;
            Capacity = newCapacity;

            return true;
        }

        public void RemoveChargeStation(Guid chargeStationId)
        {
            var station = _chargeStations.FirstOrDefault(s => s.Id == chargeStationId);
            if (station != null)
            {
                _chargeStations.Remove(station);
            }
        }

        private bool CanUpdateCapacity(int newCapacity)
        {
            return newCapacity >= GetTotalUsedCurrent();
        }

        private bool CanAddConnectors(IEnumerable<Connector> newConnectors)
        {
            var totalNewCurrent = newConnectors.Sum(c => c.MaxCurrent);
            var currentTotal = GetTotalUsedCurrent();
            return currentTotal + totalNewCurrent <= Capacity;
        }

        internal decimal GetTotalUsedCurrent()
        {
            return _chargeStations.Sum(s => s.GetCurrentLoad());
        }
    }
}