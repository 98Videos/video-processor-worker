namespace VideoProcessor.Domain.ValueObjects
{
    public abstract class Result
    {
        public bool IsSuccessful { get; private set; }

        protected Result(bool success)
        {
            IsSuccessful = success;
        }
    }

    public abstract class Result<T> : Result
        where T : class
    {
        public T? Value { get; set; }

        protected Result(bool success, T? value) : base(success)
        {
            Value = value;
        }
    }
}