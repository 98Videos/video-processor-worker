using Amazon.S3;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoProcessor.Data.S3.Options;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Domain.ValueObjects;

namespace VideoProcessor.Data.S3.Adapters
{
    public class S3FileRepository : IFileRepository
    {
        private readonly IAmazonS3 s3Client;
        private readonly S3BucketOptions options;
        private readonly ILogger<S3FileRepository> logger;

        public S3FileRepository(IAmazonS3 amazonS3,
                                IOptions<S3BucketOptions> options,
                                ILogger<S3FileRepository> logger)
        {
            this.s3Client = amazonS3;
            this.logger = logger;
            this.options = options.Value;
        }

        public async Task<VideoFile> GetVideoFile(string userEmail, string fileIdentifier)
        {
            try
            {
                logger.LogInformation("Downloading video file from s3...");

                var s3Response = await s3Client.GetObjectAsync(options.VideosBucket, $"{userEmail}/{fileIdentifier}");

                using var fileStream = s3Response.ResponseStream;
                using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);

                logger.LogInformation("file {fileIdentifier} downloaded", $"{userEmail}/{fileIdentifier}");

                var videoFile = new VideoFile(fileIdentifier, memoryStream.ToArray());
                return videoFile;
            }
            catch (Exception e)
            {
                logger.LogError(e, "could not retrieve object {fileIdentifier} for user {userEmail}", fileIdentifier, userEmail);
                throw;
            }
        }

        public async Task<Result> SaveZipFile(string userEmail, ZipFile zipFile)
        {
            try
            {
                await File.WriteAllBytesAsync($"./{zipFile.Identifier}", zipFile.Content);
                using var fileStream = new MemoryStream(zipFile.Content);

                logger.LogInformation("Saving zip file to S3...");

                await s3Client.UploadObjectFromStreamAsync(options.ZipFilesBucket,
                                                           $"{userEmail}/{zipFile.Identifier}",
                                                           fileStream,
                                                           additionalProperties: null);

                logger.LogInformation("zip file saved successfuly");

                return new SuccessResult();
            }
            catch (Exception e)
            {
                logger.LogError(e, "could not upload object {zipFile} for user {userEmail}", zipFile.Identifier, userEmail);
                throw;
            }
        }
    }
}