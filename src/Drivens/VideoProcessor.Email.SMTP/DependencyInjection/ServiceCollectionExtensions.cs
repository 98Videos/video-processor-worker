using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Email.SMTP.Adapters;
using VideoProcessor.Email.SMTP.Options;

namespace VideoProcessor.Email.SMTP.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSMTPEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SMTPSenderOptions>(configuration.GetSection(nameof(SMTPSenderOptions)));
            services.AddScoped<IEmailSender, SMTPEmailSender>();

            return services;
        }
    }
}