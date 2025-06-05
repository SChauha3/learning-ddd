namespace LearningDDD.Domain.Models
{
    internal static class ConnectorValidator
    {
        internal static void Validate(IList<Connector> connectors)
        {
            if (connectors.Count < 1 || connectors.Count > 5)
                throw new InvalidOperationException("A charge station must have between 1 and 5 connectors.");

            var duplicateContextIds = connectors
                .GroupBy(c => c.ChargeStationContextId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateContextIds.Any())
                throw new InvalidOperationException($"Duplicate connector context ID(s): {string.Join(", ", duplicateContextIds)}");
        }
    }
}
