using Microsoft.Extensions.Logging;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Domain.ValueObjects;

namespace VideoProcessor.Application.UseCases
{
    public class ExtractVideoImagesToZipUseCase
    {
        private readonly IFileRepository fileRepository;
        private readonly IVideoProcessingLibrary videoProcessingLibrary;
        private readonly IVideoManagerClient videoManagerClient;
        private readonly ILogger<ExtractVideoImagesToZipUseCase> logger;

        public ExtractVideoImagesToZipUseCase(IFileRepository fileRepository,
                                              IVideoProcessingLibrary videoProcessingLibrary,
                                              IVideoManagerClient videoManagerClient,
                                              ILogger<ExtractVideoImagesToZipUseCase> logger)

        {
            this.fileRepository = fileRepository;
            this.videoProcessingLibrary = videoProcessingLibrary;
            this.videoManagerClient = videoManagerClient;
            this.logger = logger;
        }

        public async Task<Result> Execute(string videoIdentifier, string userEmail)
        {
            logger.LogInformation("Processing video {videoId}", videoIdentifier);

            try
            {
                var videoFile = await fileRepository.GetVideoFile(userEmail, videoIdentifier);

                var images = await videoProcessingLibrary.ExtractImagesAsync(videoFile);

                var zipFile = ZipFile.Create(images);

                await fileRepository.SaveZipFile(userEmail, zipFile);

                await videoManagerClient.NotifyProcessingSuccess(videoIdentifier);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                const string errorMessage = "Could not process video";

                logger.LogError(ex, errorMessage);
                await videoManagerClient.NotifyProcessingFailure(videoIdentifier);

                return new FailureResult(errorMessage);
            }
        }
    }
}