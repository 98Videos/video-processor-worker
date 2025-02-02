namespace VideoProcessor.Clients.VideoManager.Contracts.Responses
{
    public record UpdateVideoResponse
    {
        public required string Message { get; set; }
        public required Guid VideoId { get; set; }
        public required string Status { get; set; }
    }
}
