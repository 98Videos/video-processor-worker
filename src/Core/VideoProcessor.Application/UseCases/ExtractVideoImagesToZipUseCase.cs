using Microsoft.Extensions.Logging;
using VideoProcessor.Application.Services;
using VideoProcessor.Domain.Enums;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Application.UseCases
{
    public class ExtractVideoImagesToZipUseCase
    {
        private readonly IFileManager fileManager;
        private readonly IVideoProcessingLibrary videoProcessingLibrary;
        private readonly IVideoManagerClient managerClient;
        private readonly ICompressionService compressionService;
        private readonly ILogger<ExtractVideoImagesToZipUseCase> logger;

        public ExtractVideoImagesToZipUseCase(IFileManager fileManager,
                                              IVideoProcessingLibrary videoProcessingLibrary,
                                              IVideoManagerClient managerClient,
                                              ICompressionService compressionService,
                                              ILogger<ExtractVideoImagesToZipUseCase> logger)

        {
            this.fileManager = fileManager;
            this.videoProcessingLibrary = videoProcessingLibrary;
            this.managerClient = managerClient;
            this.compressionService = compressionService;
            this.logger = logger;
        }

        public async Task Execute(string videoIdentifier, string userEmail)
        {
            try
            {
                logger.LogInformation("Processing video {videoId}", videoIdentifier);

                var videoFile = await fileManager.GetFileAsync(userEmail, videoIdentifier);

                var images = await videoProcessingLibrary.ExtractImagesAsync(videoFile);

                var zipFileName = $"{userEmail}_{videoIdentifier}.zip";
                var zipFile = compressionService.CreateZipFile(images, zipFileName);

                await fileManager.SaveNewFileAsync(zipFile);

                await managerClient.NotifyNewVideoStatusAsync(videoIdentifier, VideoStatus.Success);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not process video");
                await managerClient.NotifyNewVideoStatusAsync(videoIdentifier, VideoStatus.Failed);

                // TO DO
                // send email to user with failure
            }
        }
    }
}