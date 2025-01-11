using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Data.S3.Adapters
{
    internal class S3FileManager : IFileManager
    {
        public Task<BinaryFile> GetFileAsync(string filePath, string fileIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task SaveNewFileAsync(BinaryFile binaryFile)
        {
            throw new NotImplementedException();
        }
    }
}
