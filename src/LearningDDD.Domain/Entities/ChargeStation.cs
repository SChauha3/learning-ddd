using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Domain.Models
{
    public class ChargeStation : Entity<Guid>
    {
        private readonly List<Connector> _connectors = new();

        public string Name { get; private set; }

        public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();
        private ChargeStation() { }

        private ChargeStation(Guid id, string name, IList<Connector> connectors) : base(id)
        {
            Name = name;
            _connectors = connectors.ToList();
        }

        internal static ChargeStation Create(string name, IList<Connector> connectors) =>
            new ChargeStation(Guid.NewGuid(), name, connectors);

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