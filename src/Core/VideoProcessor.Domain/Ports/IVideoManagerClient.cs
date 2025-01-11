using VideoProcessor.Domain.Enums;

namespace VideoProcessor.Domain.Ports
{
    public interface IVideoManagerClient
    {
        Task NotifyNewVideoStatusAsync(string videoIdentifier, VideoStatus videoProcessingStatus);
    }
}
