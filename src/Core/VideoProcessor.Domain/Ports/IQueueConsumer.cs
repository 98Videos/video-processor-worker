namespace VideoProcessor.Domain.Ports
{
    public interface IQueueConsumer<T>
    {
        Task ConsumeMessage(T message);
    }
}
