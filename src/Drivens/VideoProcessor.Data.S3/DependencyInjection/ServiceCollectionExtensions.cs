using Microsoft.Extensions.DependencyInjection;

namespace VideoProcessor.Data.S3.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddS3Integration(this IServiceCollection services)
        {
            return services;
        }
    }
}
