﻿using Microsoft.Extensions.DependencyInjection;
using VideoProcessor.Application.UseCases;

namespace VideoProcessor.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped(typeof(ExtractVideoImagesToZipUseCase));

            return services;
        }
    }
}
