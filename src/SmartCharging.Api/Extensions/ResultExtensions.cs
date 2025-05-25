using SmartCharging.Api.Services;

namespace SmartCharging.Api.Extensions
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

                return Results.Created($"{uri}/{result.Value}", new { id = result.Value });
            }
                
            return Results.Problem(
            title: "Resource operation failed",
            detail: result.Error,
            statusCode: result.ErrorType switch
            {
                ErrorType.NotFound => 404,
                ErrorType.InValidCapacity => 400,
                ErrorType.UniqueConnector => 409,
                ErrorType.MinimumOneConnector => 400,
                _ => 500
            });
        }

        public static IResult ToApiResult(this Result result)
        {
            if (result.IsSuccess)
                return Results.NoContent();

            return Results.Problem(
            title: "Resource operation failed",
            detail: result.Error,
            statusCode: result.ErrorType switch
            {
                ErrorType.NotFound => 404,
                ErrorType.InValidCapacity => 400,
                ErrorType.UniqueConnector => 409,
                ErrorType.MinimumOneConnector => 400,
                _ => 500
            });
        }
    }
}