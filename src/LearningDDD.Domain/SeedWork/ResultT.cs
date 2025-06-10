namespace LearningDDD.Domain.SeedWork
{
    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public string? Error { get; }
        public ErrorType? ErrorType { get; }

        private Result(T? value, bool success, string? error, ErrorType? errorType)
        {
            Value = value;
            IsSuccess = success;
            Error = error;
            ErrorType = errorType;
        }

        public static Result<T> Success(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Success result must have a value.");

            return new(value, true, null, null);
        }

        public static Result<T> Fail(string error, ErrorType errorType) =>
            new(default, false, error, errorType);
    }
}