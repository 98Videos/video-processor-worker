using Microsoft.Extensions.Logging;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Application.UseCases
{
    public class ExtractVideoImagesToZipUseCase
    {
        private readonly IFileRepository fileRepository;
        private readonly IVideoProcessingLibrary videoProcessingLibrary;
        private readonly IVideoManagerClient managerClient;
        private readonly ILogger<ExtractVideoImagesToZipUseCase> logger;

        public ExtractVideoImagesToZipUseCase(IFileRepository fileRepository,
                                              IVideoProcessingLibrary videoProcessingLibrary,
                                              IVideoManagerClient managerClient,
                                              ILogger<ExtractVideoImagesToZipUseCase> logger)

        {
            this.fileRepository = fileRepository;
            this.videoProcessingLibrary = videoProcessingLibrary;
            this.managerClient = managerClient;
            this.logger = logger;
        }

        public async Task Execute(string videoIdentifier, string userEmail)
        {
            logger.LogInformation("Processing video {videoId}", videoIdentifier);

            try
            {
                var videoFile = await fileRepository.GetVideoFile(userEmail, videoIdentifier);

                var images = await videoProcessingLibrary.ExtractImagesAsync(videoFile);

                var zipFile = ZipFile.Create(images);

                await fileRepository.SaveZipFile(userEmail, zipFile);

                await managerClient.NotifyProcessingSuccess(videoIdentifier);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not process video");
                await managerClient.NotifyProcessingFailure(videoIdentifier);

                // TO DO
                // send email to user with failure
            }
        }
    }
}