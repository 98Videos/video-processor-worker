namespace VideoProcessor.Domain.Ports
{
    public interface IVideoManagerClient
    {
        Task NotifyProcessingSuccess(string videoIdentifier);

        Task NotifyProcessingFailure(string videoIdentifier);
    }
}