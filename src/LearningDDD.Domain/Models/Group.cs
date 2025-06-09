using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Domain.Models
{
    public class Group : Entity<Guid>, IAggregateRoot
    {
        private readonly List<ChargeStation> _chargeStations = new();
        public string Name { get; private set; }
        public int Capacity { get; private set; }

        public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations.AsReadOnly();

        private Group(Guid id, string name, int capacity) : base(id)
        {
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

        public bool TryUpdate(string name, int newCapacity)
        {
            if (!CanUpdateCapacity(newCapacity))
                return false;

            Name = name;
            Capacity = newCapacity;

            return true;
        }

        public Guid AddChargeStation(string name, IEnumerable<(int chargeStationContextId, int maxCurrent)> connectors)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Charge station name is required.");

            if (connectors == null)
                throw new ArgumentNullException(nameof(connectors));

            var newConnectors = connectors.Select(c => Connector.Create(c.chargeStationContextId, c.maxCurrent)).ToList();

            ConnectorValidator.Validate(newConnectors);

            var chargeStation = ChargeStation.Create(name, newConnectors);

            _chargeStations.Add(chargeStation);
            return chargeStation.Id;
        }

        public void UpdateChargeStation(Guid chargeStationId, string newName)
        {
            var chargeStation = _chargeStations.FirstOrDefault(cs => cs.Id == chargeStationId);
            if (chargeStation is null)
                throw new ArgumentException("Charge station with id {chargeStationId} not found in this group.");

            // Add Group-level invariants here if needed (e.g., no duplicate names within the group)
            if (_chargeStations.Where(cs => cs.Id != chargeStationId).Any(cs => cs.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"A charge station with name '{newName}' already exists in this group.");
            
            // Delegate the update to the ChargeStation entity itself
            chargeStation.UpdateName(newName);
        }

        public void RemoveChargeStation(Guid chargeStationId)
        {
            var station = _chargeStations.FirstOrDefault(s => s.Id == chargeStationId);
            if (station is not null)
                _chargeStations.Remove(station);
        }

        private bool CanUpdateCapacity(int newCapacity) =>
            newCapacity >= GetTotalUsedCurrent();


        //private bool CanAddConnectors(IEnumerable<Connector> newConnectors)
        //{
        //    var totalNewCurrent = newConnectors.Sum(c => c.MaxCurrent);
        //    var currentTotal = GetTotalUsedCurrent();
        //    return currentTotal + totalNewCurrent <= Capacity;
        //}

        public bool CanAddConnector(int maxCurrent) =>
            GetTotalUsedCurrent() + maxCurrent < Capacity;
        

        private decimal GetTotalUsedCurrent() =>
            _chargeStations.Sum(s => s.GetCurrentLoad());

        public bool IsChargeStationContextIdUnique(int ChargeStationContextId, Guid chargeStationId)
        {
            var chargeStation = _chargeStations.FirstOrDefault(cs => cs.Id == chargeStationId);
            if (chargeStation is null)
                throw new ArgumentException("Charge station with id {chargeStationId} not found in this group.");

            return !chargeStation.Connectors.Where(c => c.ChargeStationContextId == ChargeStationContextId).Any();
        }

        public Guid AddConnectorToChargeStation(int chargeStationContextId, int maxCurrent, Guid chargeStationId)
        {
            var chargeStation = ChargeStations.Where(cs => cs.Id == chargeStationId).FirstOrDefault();
            var connector = Connector.Create(chargeStationContextId, maxCurrent);
            chargeStation?.AddConnector(connector);
            return connector.Id;
        }

        public bool CanUpdateMaxCurrent(int maxCurrent, int existingConnectorCurrent)
        {
            return Capacity > GetTotalUsedCurrent() + maxCurrent - existingConnectorCurrent;
        }

        public bool UpdateConnectorMaxCurrent(int maxCurrent, Guid chargeStationId, Guid connectorId)
        {
            var chargeStation = ChargeStations.Where(cs => cs.Id == chargeStationId).FirstOrDefault();
            var connector = chargeStation?.Connectors.Where(c => c.Id == connectorId).FirstOrDefault();

            if (!CanUpdateMaxCurrent(maxCurrent, connector.MaxCurrent))
                return false;

            connector?.UpdateMaxCurrent(maxCurrent);
            return true;
        }
    }
}