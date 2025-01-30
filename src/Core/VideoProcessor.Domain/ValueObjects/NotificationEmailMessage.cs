namespace VideoProcessor.Domain.ValueObjects
{
    public class NotificationEmailMessage(string recipient, string subject, string body)
    {
        public string Subject { get; set; } = $"98 Videos Notification - {subject}";

        public string To { get; set; } = recipient;

        public string Body { get; } = body;
    }
}