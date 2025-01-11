namespace VideoProcessor.Messaging.SQS.Contracts
{
    internal record VideoToProcessMessage
    {
        public required string VideoId { get; set; }
        public required string UserEmail { get; set; }
    }
}
