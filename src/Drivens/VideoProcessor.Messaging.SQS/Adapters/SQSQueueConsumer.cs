using MassTransit;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Messaging.SQS.Contracts;

namespace VideoProcessor.Messaging.SQS.Adapters
{
    internal class SQSQueueConsumer : IQueueConsumer<VideoToProcessMessage>, IConsumer<VideoToProcessMessage>
    {
        public SQSQueueConsumer()
        {
            
        }

        public Task Consume(ConsumeContext<VideoToProcessMessage> context)
        {
            throw new NotImplementedException();
        }

        public Task ConsumeMessage(VideoToProcessMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
