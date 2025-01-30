using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Domain.ValueObjects;
using VideoProcessor.Email.SMTP.Options;

namespace VideoProcessor.Email.SMTP.Adapters
{
    public class SMTPEmailSender : IEmailSender
    {
        private readonly SMTPSenderOptions smtpOptions;
        private readonly SmtpClient smtpClient;
        private readonly ILogger<SMTPEmailSender> logger;

        public SMTPEmailSender(IOptions<SMTPSenderOptions> options, ILogger<SMTPEmailSender> logger)
        {
            smtpOptions = options.Value;

            this.smtpClient = new SmtpClient(smtpOptions.Host, smtpOptions.Port)
            {
                Credentials = new NetworkCredential(smtpOptions.User, smtpOptions.Password),
                EnableSsl = true
            };

            this.logger = logger;
        }

        public async Task<Result> SendNotificationEmail(NotificationEmailMessage emailMessage)
        {
            try
            {
                logger.LogInformation("sending email to {recipient}", emailMessage.To);

                await smtpClient.SendMailAsync(smtpOptions.FromEmail, emailMessage.To, emailMessage.Subject, emailMessage.Body);

                logger.LogInformation("sent successfuly");

                return new SuccessResult();
            }
            catch (Exception e)
            {
                const string errorMessage = "error sending email";

                logger.LogError(e, errorMessage);
                return new FailureResult(errorMessage);
            }
        }
    }
}