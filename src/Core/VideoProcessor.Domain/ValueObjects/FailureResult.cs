namespace VideoProcessor.Domain.ValueObjects
{
    public class FailureResult : Result
    {
        public FailureResult(string errorMessage) : base(false)
        {
            Error = new FailureResultError(errorMessage);
        }

        public FailureResultError Error { get; private set; }
    }

    public class FailureResult<T> : Result<T>
        where T : class
    {
        public FailureResult(string errorMessage) : base(false, null)
        {
            Error = new FailureResultError(errorMessage);
        }

        public FailureResultError Error { get; private set; }
    }

    public class FailureResultError(string message)
    {
        public string Message { get; private set; } = message;
    }
}