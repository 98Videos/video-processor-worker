using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using MassTransit;
using Serilog;
using Serilog.Events;
using VideoProcessor.Worker.Consumers;
using VideoProcessor.Worker.Options;

namespace VideoProcessor.Worker.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSQSMessageConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new ConsumerOptions();
            configuration.GetRequiredSection(nameof(ConsumerOptions)).Bind(options);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<VideosToProcessConsumer>();

                x.UsingAmazonSqs((context, cfg) =>
                {
                    var credentialChain = new CredentialProfileStoreChain();
                    if (!credentialChain.TryGetAWSCredentials("default", out AWSCredentials awsCredentials))
                    {
                        awsCredentials = new EnvironmentVariablesAWSCredentials();
                    }

                    cfg.Host("us-east-1", h =>
                    {
                        h.Credentials(awsCredentials);
                    });

                    cfg.ReceiveEndpoint(options.QueueName, e =>
                    {
                        e.ConcurrentMessageLimit = options.ConcurrentMessageLimit;
                        e.ConfigureConsumer<VideosToProcessConsumer>(context);
                    });
                });
            });
            return services;
        }

        public static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddSerilog(cfg =>
            {
                cfg
                    .WriteTo.Console()
                    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
            });

            return services;
        }
    }
}