namespace VideoProcessor.Worker.Options
{
    public class ConsumerOptions
    {
        public string QueueName { get; set; } = null!;

        public int ConcurrentMessageLimit { get; set; }
    }
}
