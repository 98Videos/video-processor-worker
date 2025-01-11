using Microsoft.Extensions.Logging;
using System.IO.Compression;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Enums;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Application.UseCases
{
    public class ExtractVideoImagesToZipUseCase
    {
        private readonly IFileManager fileManager;
        private readonly IVideoProcessingLibrary videoProcessingLibrary;
        private readonly IVideoManagerClient managerClient;
        private readonly ILogger<ExtractVideoImagesToZipUseCase> logger;

        public ExtractVideoImagesToZipUseCase(IFileManager fileManager,
                                              IVideoProcessingLibrary videoProcessingLibrary,
                                              IVideoManagerClient managerClient,
                                              ILogger<ExtractVideoImagesToZipUseCase> logger)

        {
            this.fileManager = fileManager;
            this.videoProcessingLibrary = videoProcessingLibrary;
            this.managerClient = managerClient;
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
                var zipFile = CreateZipFile(images, zipFileName);

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

        private static BinaryFile CreateZipFile(IEnumerable<BinaryFile> imageFiles, string outputZipFileName)
        {
            using var zipFileStream = new MemoryStream();
            var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create);

            foreach (var imageFile in imageFiles)
            {
                var zipEntry = zipArchive.CreateEntry(imageFile.Identifier);
                using var imageFileStream = new MemoryStream(imageFile.File);
                using var zipFileEntryStream = zipEntry.Open();

                imageFileStream.CopyTo(zipFileEntryStream);
            }

            return new BinaryFile()
            {
                Identifier = outputZipFileName,
                File = zipFileStream.ToArray()
            };
        }
    }
}