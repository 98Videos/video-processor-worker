using Microsoft.Extensions.DependencyInjection;

namespace VideoProcessor.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {

            return services;
        }
    }
}
