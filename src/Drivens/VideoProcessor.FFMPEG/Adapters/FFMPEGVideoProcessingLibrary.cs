using FFMpegCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.FFMPEG.Adapters
{
    public class FFMPEGVideoProcessingLibrary : IVideoProcessingLibrary
    {
        private const int intervalSeconds = 20;
        private readonly ILogger<FFMPEGVideoProcessingLibrary> logger;

        public FFMPEGVideoProcessingLibrary(ILogger<FFMPEGVideoProcessingLibrary> logger)
        {
            this.logger = logger;

            GlobalFFOptions.Current.BinaryFolder = "LibraryBinaries";
        }

        public async Task<IEnumerable<ImageFile>> ExtractImagesAsync(ProcessFile videoFile)
        {
            string videoPath = Path.Combine(Path.GetTempPath(), videoFile.Identifier);
            string outputDirectory = Path.Combine(Path.GetTempPath(), "frames");

            try
            {
                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);

                await File.WriteAllBytesAsync(videoPath, videoFile.Content);

                var videoInfo = await FFProbe.AnalyseAsync(videoPath);
                var videoDuration = videoInfo.Duration;

                logger.LogInformation("video duration: {videoDuration}", videoDuration);

                for (var currentTime = TimeSpan.Zero; currentTime < videoDuration; currentTime += TimeSpan.FromSeconds(intervalSeconds))
                {
                    logger.LogInformation("Processing frame {hour}:{minute}:{second}", currentTime.Hours, currentTime.Minutes, currentTime.Seconds);

                    var outputPath = Path.Combine(outputDirectory, $"frame_at_{currentTime.TotalSeconds}.jpg");
                    await FFMpeg.SnapshotAsync(videoPath, outputPath, new Size(1920, 1080), currentTime);
                }

                var allImageFiles = Directory.GetFiles(outputDirectory);
                var imageFileList = new List<ImageFile>();

                var originalVideoIdentifier = Path.GetFileNameWithoutExtension(videoFile.Identifier);
                foreach (var imageFilePath in allImageFiles)
                {
                    var imageFileBytes = await File.ReadAllBytesAsync(imageFilePath);
                    var fileName = Path.GetFileName(imageFilePath);

                    imageFileList.Add(new ImageFile(fileName, imageFileBytes, originalVideoIdentifier));

                    File.Delete(imageFilePath);
                }

                File.Delete(videoPath);
                return imageFileList;
            }
            catch (Exception e)
            {
                logger.LogError(e, "could not extract images from video");

                if (File.Exists(videoPath))
                    File.Delete(videoPath);

                if (Directory.Exists(outputDirectory))
                    Directory.Delete(outputDirectory);

                throw;
            }
        }
    }
}