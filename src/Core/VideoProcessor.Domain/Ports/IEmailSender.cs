using VideoProcessor.Domain.ValueObjects;

namespace VideoProcessor.Domain.Ports
{
    public interface IEmailSender
    {
        public Task<Result> SendNotificationEmail(NotificationEmailMessage emailMessage);
    }
}