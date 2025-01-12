using Amazon.S3;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoProcessor.Data.S3.Options;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

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
                var s3Response = await s3Client.GetObjectAsync(options.VideosBucket, $"{userEmail}/{fileIdentifier}");

                using var fileStream = s3Response.ResponseStream;
                using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);

                return new VideoFile(fileIdentifier, memoryStream.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "could not retrieve object {fileIdentifier} for user {userEmail}", fileIdentifier, userEmail);
                throw;
            }
        }

        public async Task SaveZipFile(string userEmail, ZipFile file)
        {
            await File.WriteAllBytesAsync($"./{file.Identifier}", file.Content);
            using var fileStream = new MemoryStream(file.Content);

            await s3Client.UploadObjectFromStreamAsync(options.ZipFilesBucket,
                                                       $"{userEmail}/{file.Identifier}",
                                                       fileStream,
                                                       additionalProperties: null);
        }
    }
}