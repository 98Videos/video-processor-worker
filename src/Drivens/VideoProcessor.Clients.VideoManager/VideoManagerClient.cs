using VideoProcessor.Domain.Enums;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Clients.VideoManager
{
    public class VideoManagerClient : IVideoManagerClient
    {
        private readonly HttpClient httpClient;

        public VideoManagerClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task NotifyProcessingFailure(string videoIdentifier)
        {
            var newStatus = VideoProcessingStatus.Success;
            await CallServiceWithNewStatus(videoIdentifier, newStatus);
        }

        public async Task NotifyProcessingSuccess(string videoIdentifier)
        {
            var newStatus = VideoProcessingStatus.Success;
            await CallServiceWithNewStatus(videoIdentifier, newStatus);
        }

        private Task CallServiceWithNewStatus(string videoIdentifier, VideoProcessingStatus newStatus)
        {
            // TODO: Call service when endpoint is implemented

            return Task.CompletedTask;
        }
    }
}