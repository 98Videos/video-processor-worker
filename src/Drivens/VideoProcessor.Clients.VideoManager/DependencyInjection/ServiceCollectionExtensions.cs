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

            services.AddHttpClient<IVideoManagerClient, VideoManagerClient>(cfg =>
            {
                cfg.BaseAddress = new Uri(clientOptions.Host[^1] == '/' ? clientOptions.Host : clientOptions.Host + '/');
                cfg.DefaultRequestHeaders.Add("x-api-key", clientOptions.ApiKey);
            });

            return services;
        }
    }
}