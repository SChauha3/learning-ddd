using LearningDDD.Domain.SeedWork;

namespace LearningDDD.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiResult<T>(this Result<T> result, string uri, bool isCreated = false)
        {
            if (result.IsSuccess)
            {
                if (!isCreated)
                {
                    return Results.Ok(result.Value);
                }

                return Results.Created($"{uri}/{result.Value}", result.Value);
            }

            return Results.Problem(
            title: "Resource operation failed",
            detail: result.Error,
            statusCode: result.ErrorType switch
            {
                ErrorType.GroupNotFound => 404,
                ErrorType.ChargeStationNotFound => 400,
                ErrorType.ConnectorNotFound => 400,
                ErrorType.GroupNameRequired => 400,
                ErrorType.ChargeStationNameRequired => 400,
                ErrorType.ChargeStationNameMustBeUnique => 400,
                ErrorType.ChargeStationWithoutConnector => 400,
                ErrorType.ConnectorNameRequired => 400,
                ErrorType.CapacityNotGreaterThanZero => 400,
                ErrorType.CapacityNotGreaterThanUsedCurrent => 400,
                ErrorType.UniqueConnector => 409,
                _ => 500
            });
        }
    }
}