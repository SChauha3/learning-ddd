using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Domain.Models
{
    public class ChargeStation : Entity<Guid>
    {
        private readonly List<Connector> _connectors = new();

        public string Name { get; private set; }

        public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();
        private ChargeStation() { }

        private ChargeStation(Guid id, string name, IEnumerable<(int chargeStationContextId, int maxCurrent)> connectors) : base(id)
        {
            Name = name;
            _connectors = connectors.Select(c => Connector.Create(c.chargeStationContextId, c.maxCurrent)).ToList();
            ConnectorValidator.Validate(_connectors);
        }

        internal static ChargeStation Create(string name, IEnumerable<(int chargeStationContextId, int maxCurrent)> connectors) =>
            new ChargeStation(Guid.Empty, name, connectors);

        internal void UpdateName(string name)
        {
            Name = name;
        }

        internal int GetCurrentLoad() =>
            Connectors.Sum(c => c.MaxCurrent);

        internal void AddConnector(Connector connector) =>
            _connectors.Add(connector);

        internal Result<Connector> RemoveConnector(Guid connectorId)
        {
            var connector = Connectors.FirstOrDefault(c => c.Id == connectorId);
            if (connector is null)
                return Result<Connector>.Fail("Connector cannot be null.", ErrorType.ConnectorNotFound);

            _connectors.Remove(connector);
            return Result<Connector>.Success(connector);
        }
    }
}