namespace LearningDDD.Domain.SeedWork
{
    public class Error
    {
        public string Message { get; }
        public ErrorType? ErrorType { get; }

        public static readonly Error None = new(string.Empty, null);

        public Error(string message, ErrorType? errorType = null)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            ErrorType = errorType;
        }

        public bool IsNone => string.IsNullOrEmpty(Message) && ErrorType == null;

        public static explicit operator Error(string message) => new(message);
        public static explicit operator string(Error error) => error.Message;
    }

    
}
