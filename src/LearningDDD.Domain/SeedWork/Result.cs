namespace LearningDDD.Domain.SeedWork
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public ErrorType? ErrorType { get; }

        private Result(bool isSuccess, string? error, ErrorType? errorType)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success() =>
            new(true, null, null);

        public static Result Fail(string error, ErrorType errorType) =>
            new(false, error, errorType);
    }
}