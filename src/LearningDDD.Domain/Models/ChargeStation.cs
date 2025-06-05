namespace LearningDDD.Domain.Models
{
    public class ChargeStation
    {
        private readonly List<Connector> _connectors = new();

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();

        private ChargeStation(string name, IList<Connector> connectors)
        {
            Id = Guid.NewGuid();
            Name = name;
            _connectors = connectors.ToList();
        }

        internal static ChargeStation Create(string name , IList<Connector> connectors) =>
            new ChargeStation(name, connectors);
            
        internal void Update(string name)
        {
            Name = name;
        }

        internal int GetCurrentLoad() =>
            Connectors.Sum(c => c.MaxCurrent);
    }
}