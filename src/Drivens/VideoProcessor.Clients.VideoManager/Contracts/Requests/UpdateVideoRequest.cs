using VideoProcessor.Domain.Enums;

namespace VideoProcessor.Clients.VideoManager.Contracts.Requests
{
    public record UpdateVideoRequest
    {
        public VideoStatus Status { get; set; }
    }
}