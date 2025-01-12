using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoProcessor.Data.S3.Adapters;
using VideoProcessor.Data.S3.Options;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Data.S3.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddS3FileManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<S3BucketOptions>(configuration.GetSection(nameof(S3BucketOptions)));

            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            services.AddScoped<IFileRepository, S3FileRepository>();

            return services;
        }
    }
}
