using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoProcessor.Clients.VideoManager.Options;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Clients.VideoManager.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVideoManagerClient(this IServiceCollection services, IConfiguration config)
        {
            var clientOptions = new VideoManagerClientOptions();
            config.Bind(nameof(VideoManagerClientOptions), clientOptions);

            services.AddHttpClient<VideoManagerClient>(cfg =>
            {
                cfg.BaseAddress = new Uri(clientOptions.Host);
            });

            services.AddScoped<IVideoManagerClient, VideoManagerClient>();

            return services;
        }
    }
}