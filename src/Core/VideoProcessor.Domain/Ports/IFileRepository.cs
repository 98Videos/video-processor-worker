using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Domain.Ports
{
    public interface IFileRepository
    {
        Task<VideoFile> GetVideoFile(string userEmail, string fileIdentifier);

        Task SaveZipFile(string userEmail, ZipFile file);
    }
}