using Amazon.S3;
using Microsoft.Extensions.Options;
using VideoProcessor.Data.S3.Options;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Data.S3.Adapters
{
    internal class S3FileRepository : IFileRepository
    {
        private readonly IAmazonS3 amazonS3Client;
        private readonly S3BucketOptions options;

        public S3FileRepository(IAmazonS3 amazonS3, IOptions<S3BucketOptions> options)
        {
            this.amazonS3Client = amazonS3;
            this.options = options.Value;
        }

        public async Task<VideoFile> GetVideoFile(string userEmail, string fileIdentifier)
        {
            var s3Response = await amazonS3Client.GetObjectAsync(options.VideosBucket, $"{userEmail}/{fileIdentifier}");

            using var fileStream = s3Response.ResponseStream;
            using var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(memoryStream);

            return new VideoFile(fileIdentifier, memoryStream.ToArray());
        }

        public Task SaveZipFile(string userEmail, ZipFile file)
        {
            throw new NotImplementedException();
        }
    }
}