using Microsoft.Extensions.DependencyInjection;

namespace VideoProcessor.Messaging.SQS.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSQSMessaging(this IServiceCollection services)
        {
            return services;
        }
    }
}
