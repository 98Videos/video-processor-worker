using Amazon.Runtime;
using MassTransit;
using VideoProcessor.Application.DependencyInjection;
using VideoProcessor.Clients.VideoManager.DependencyInjection;
using VideoProcessor.Data.S3.DependencyInjection;
using VideoProcessor.FFMPEG.DependencyInjection;
using VideoProcessor.Worker.Consumers;
using VideoProcessor.Worker.Contracts;

var builder = WebApplication.CreateSlimBuilder();

builder.Services.AddHealthChecks();
builder.WebHost.UseKestrelHttpsConfiguration();

builder.Services.AddS3FileManager(builder.Configuration);
builder.Services.AddFFMEGVideoProcessingLibrary();
builder.Services.AddVideoManagerClient(builder.Configuration);
builder.Services.AddUseCases();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<VideosToProcessConsumer>();

    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.Credentials(new EnvironmentVariablesAWSCredentials());
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