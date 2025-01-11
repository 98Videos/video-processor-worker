using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Domain.Ports
{
    public interface IFileManager
    {
        Task<BinaryFile> GetFileAsync(string filePath, string fileIdentifier);

        Task SaveNewFileAsync(BinaryFile binaryFile);
    }
}