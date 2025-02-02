using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using VideoProcessor.Clients.VideoManager.Contracts.Requests;
using VideoProcessor.Domain.Enums;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Clients.VideoManager
{
    public class VideoManagerClient : IVideoManagerClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<VideoManagerClient> logger;

        public VideoManagerClient(HttpClient httpClient, ILogger<VideoManagerClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task NotifyProcessingFailure(string videoIdentifier)
        {
            var newStatus = VideoStatus.Falha;
            await CallServiceWithNewStatus(videoIdentifier, newStatus);
        }

        public async Task NotifyProcessingSuccess(string videoIdentifier)
        {
            var newStatus = VideoStatus.Processado;
            await CallServiceWithNewStatus(videoIdentifier, newStatus);
        }

        private async Task CallServiceWithNewStatus(string videoIdentifier, VideoStatus newStatus)
        {
            try
            {
                var request = new UpdateVideoRequest()
                {
                    Status = newStatus
                };

                logger.LogInformation("notifying media manager of new video status {newStatus} on video {videoIdentifier}", newStatus, videoIdentifier);
                var resp = await httpClient.PutAsJsonAsync($"api/videos/{videoIdentifier}/status", request);
                resp.EnsureSuccessStatusCode();

                logger.LogInformation("successfully updated {videoIdentifier} status!", videoIdentifier);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "error updateding {videoIdentifier} status! Status code was {statusCode}.", videoIdentifier, e.StatusCode);
                throw;
            }
        }
    }
}