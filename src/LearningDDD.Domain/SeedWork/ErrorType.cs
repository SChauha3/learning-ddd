namespace LearningDDD.Domain.SeedWork
{
    public enum ErrorType
    {
        GroupNotFound,
        GroupNameRequired,

        ChargeStationNotFound,
        ChargeStationNameRequired,
        ChargeStationNameMustBeUnique,
        ChargeStationWithoutConnector,

        ConnectorNotFound,
        ConnectorNameRequired,
        CapacityNotGreaterThanZero,
        UniqueConnector,
        CapacityNotGreaterThanUsedCurrent,

        InvalidId,
        Unknown
    }
}
