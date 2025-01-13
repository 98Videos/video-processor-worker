namespace VideoProcessor.Worker.Contracts
{
    public class VideoToProcessMessage
    {
        public string VideoId { get; set; }
        public string UserEmail { get; set; }
    }
}
