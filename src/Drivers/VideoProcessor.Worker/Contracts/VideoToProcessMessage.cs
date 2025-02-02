using MassTransit;

namespace VideoProcessor.Worker.Contracts
{
    [MessageUrn("video-to-process-message")]
    public class VideoToProcessMessage
    {
        public string VideoId { get; set; }
        public string UserEmail { get; set; }
    }
}
