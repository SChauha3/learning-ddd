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

        public static Result<Group> Create(string name, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Group>.Fail("Group name is required.", ErrorType.GroupNameRequired);
            if (capacity <= 0)
                return Result<Group>.Fail("Capacity must be greater than zero.", ErrorType.CapacityNotGreaterThanZero);

            return Result<Group>.Success(new Group(Guid.NewGuid(), name, capacity));
        }

        public Result<bool> Update(string name, int newCapacity)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<bool>.Fail("Group name is required.", ErrorType.GroupNameRequired);
            if (!CanUpdateCapacity(newCapacity))
                Result<bool>.Fail("New capacity is less than the total used current.", ErrorType.CapacityNotGreaterThanUsedCurrent);

            Name = name;
            Capacity = newCapacity;

            return Result<bool>.Success(true);
        }

        public Result<ChargeStation> AddChargeStation(
            string name,
            IEnumerable<(int chargeStationContextId, int maxCurrent)> connectors)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<ChargeStation>.Fail("Charge station name is required.", ErrorType.ChargeStationNameRequired);

            if (!connectors.Any())
                return Result<ChargeStation>.Fail("Connectors cannot be null.", ErrorType.ChargeStationWithoutConnector);

            var chargeStation = ChargeStation.Create(name, connectors);

            _chargeStations.Add(chargeStation);
            return Result<ChargeStation>.Success(chargeStation);
        }

        public Result<bool> UpdateChargeStation(Guid chargeStationId, string newName)
        {
            var chargeStation = _chargeStations.FirstOrDefault(cs => cs.Id == chargeStationId);

            if (chargeStation is null)
                return Result<bool>.Fail(
                    "Charge station with id {chargeStationId} not found in this group.",
                    ErrorType.ChargeStationNotFound);

            if (!IsChargeStationNameUnique(newName))
                Result<bool>.Fail(
                    $"A charge station with name '{newName}' already exists in this group.",
                    ErrorType.ChargeStationNameMustBeUnique);

            chargeStation.UpdateName(newName);
            return Result<bool>.Success(true);
        }

        public Result<ChargeStation> RemoveChargeStation(Guid chargeStationId)
        {
            var chargeStation = _chargeStations.FirstOrDefault(s => s.Id == chargeStationId);
            if (chargeStation is null)
                return Result<ChargeStation>.Fail("Charge station not found.", ErrorType.ChargeStationNotFound);

            _chargeStations.Remove(chargeStation);
            return Result<ChargeStation>.Success(chargeStation);
        }

        public Result<Connector> AddConnectorToChargeStation(int chargeStationContextId, int maxCurrent, Guid chargeStationId)
        {
            var chargeStation = ChargeStations.Where(cs => cs.Id == chargeStationId).FirstOrDefault();

            if (chargeStation is null)
                return Result<Connector>.Fail(
                    $"Charge station with id {chargeStationId} not found.",
                    ErrorType.ChargeStationNotFound);

            if (!CanAddConnector(maxCurrent))
                return Result<Connector>.Fail(
                    "Cannot add connector. The total current exceeds the group's capacity.",
                    ErrorType.CapacityNotGreaterThanUsedCurrent);

            if (!IsChargeStationContextIdUnique(chargeStationContextId, chargeStation))
                return Result<Connector>.Fail("Id must be unique within the context of a charge station with " +
                    "(possible range of values from 1 to 5)", ErrorType.UniqueConnector);

            var connector = Connector.Create(chargeStationContextId, maxCurrent);
            chargeStation?.AddConnector(connector);
            return Result<Connector>.Success(connector);
        }

        public Result<bool> UpdateConnector(int maxCurrent, Guid chargeStationId, Guid connectorId)
        {
            var chargeStation = ChargeStations.Where(cs => cs.Id == chargeStationId).FirstOrDefault();
            if (chargeStation is null)
                return Result<bool>.Fail(
                    $"Charge station with id {chargeStationId} not found.",
                    ErrorType.ChargeStationNotFound);

            var connector = chargeStation?.Connectors.Where(c => c.Id == connectorId).FirstOrDefault();
            if (connector is null)
                return Result<bool>.Fail(
                    $"Connector with id {connectorId} not found in charge station {chargeStationId}.",
                    ErrorType.ConnectorNotFound);

            if (!CanUpdateMaxCurrent(maxCurrent, connector.MaxCurrent))
                return Result<bool>.Fail(
                    "Cannot update connector max current. The total current exceeds the group's capacity.",
                    ErrorType.CapacityNotGreaterThanUsedCurrent);

            connector?.Update(maxCurrent);
            return Result<bool>.Success(true);
        }

        public Result<Connector> RemoveConnector(Guid connectorId, Guid chargeStationId)
        {
            var chargeStation = _chargeStations.FirstOrDefault(cs => cs.Id == chargeStationId);
            if (chargeStation is null)
                return Result<Connector>.Fail("Charge station not found.", ErrorType.ChargeStationNotFound);

            return chargeStation.RemoveConnector(connectorId);
        }

        private bool CanUpdateMaxCurrent(int maxCurrent, int existingConnectorCurrent) =>
            Capacity > GetTotalUsedCurrent() + maxCurrent - existingConnectorCurrent;

        private bool CanUpdateCapacity(int newCapacity) =>
            newCapacity >= GetTotalUsedCurrent();

        private bool CanAddConnector(int maxCurrent) =>
            GetTotalUsedCurrent() + maxCurrent < Capacity;

        private decimal GetTotalUsedCurrent() =>
            _chargeStations.Sum(s => s.GetCurrentLoad());

        private bool IsChargeStationContextIdUnique(int ChargeStationContextId, ChargeStation chargeStation) =>
            !chargeStation.Connectors.Where(c => c.ChargeStationContextId == ChargeStationContextId).Any();

        private bool IsChargeStationNameUnique(string newName) =>
            _chargeStations.Any(cs => cs.Name.Equals(newName, StringComparison.OrdinalIgnoreCase));
    }
}