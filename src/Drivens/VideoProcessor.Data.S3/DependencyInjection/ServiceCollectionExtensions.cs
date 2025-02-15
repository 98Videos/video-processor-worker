﻿using Amazon;
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

            var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

            services.AddSingleton<IAmazonS3>(s3Client);
            services.AddScoped<IFileRepository, S3FileRepository>();

            return services;
        }
    }
}