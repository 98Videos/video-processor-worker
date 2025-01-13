using MassTransit;
using VideoProcessor.Application.UseCases;
using VideoProcessor.Worker.Contracts;

namespace VideoProcessor.Worker.Consumers
{
    public class VideosToProcessConsumer : IConsumer<VideoToProcessMessage>
    {
        private readonly ExtractVideoImagesToZipUseCase extractVideoImagesToZipUseCase;
        private readonly ILogger<VideosToProcessConsumer> logger;

        public VideosToProcessConsumer(ExtractVideoImagesToZipUseCase extractVideoImagesToZipUseCase, ILogger<VideosToProcessConsumer> logger)
        {
            this.extractVideoImagesToZipUseCase = extractVideoImagesToZipUseCase;
            this.logger = logger;
        }

        async Task IConsumer<VideoToProcessMessage>.Consume(ConsumeContext<VideoToProcessMessage> context)
        {
            var videoIdentifier = context.Message.VideoId;
            var userEmail = context.Message.UserEmail;

            logger.LogInformation("New Message! Starting to process {videoIdentifier}", videoIdentifier);

            await extractVideoImagesToZipUseCase.Execute(videoIdentifier, userEmail);

            logger.LogInformation("Message processed successfuly");
        }
    }
}