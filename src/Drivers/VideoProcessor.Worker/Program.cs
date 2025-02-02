using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using MassTransit;
using VideoProcessor.Application.DependencyInjection;
using VideoProcessor.Clients.VideoManager.DependencyInjection;
using VideoProcessor.Data.S3.DependencyInjection;
using VideoProcessor.Email.SMTP.DependencyInjection;
using VideoProcessor.FFMPEG.DependencyInjection;
using VideoProcessor.Worker.Consumers;

var builder = WebApplication.CreateSlimBuilder();

builder.Services.AddHealthChecks();
builder.WebHost.UseKestrelHttpsConfiguration();

builder.Services
    .AddS3FileManager(builder.Configuration)
    .AddFFMEGVideoProcessingLibrary()
    .AddVideoManagerClient(builder.Configuration)
    .AddSMTPEmailSender(builder.Configuration)
    .AddUseCases();

builder.Services.AddMassTransit(x =>
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

        cfg.ReceiveEndpoint("videos-to-process", e =>
        {
            e.ConcurrentMessageLimit = 1;
            e.ConfigureConsumer<VideosToProcessConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();