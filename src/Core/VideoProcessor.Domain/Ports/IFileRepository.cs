using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.ValueObjects;

namespace VideoProcessor.Domain.Ports
{
    public interface IFileRepository
    {
        Task<VideoFile> GetVideoFile(string userEmail, string fileIdentifier);

        Task<Result> SaveZipFile(string userEmail, ZipFile file);
    }
}