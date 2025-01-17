namespace VideoProcessor.Domain.ValueObjects
{
    public class SuccessResult : Result
    {
        public SuccessResult() : base(true)
        {
        }
    }

    public class SuccessResult<T> : Result<T>
        where T : class
    {
        public SuccessResult(T value) : base(true, value)
        {
        }
    }
}