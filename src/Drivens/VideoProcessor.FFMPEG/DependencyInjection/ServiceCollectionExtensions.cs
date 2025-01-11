using Microsoft.Extensions.DependencyInjection;
using VideoProcessor.Domain.Ports;
using VideoProcessor.FFMPEG.Adapters;

namespace VideoProcessor.FFMPEG.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFFMEGVideoProcessingLibrary(this IServiceCollection services)
        {
            services.AddSingleton<IVideoProcessingLibrary, FFMPEGVideoProcessingLibrary>();

            return services;
        }
    }
}
