using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using VideoProcessor.Application.DependencyInjection;
using VideoProcessor.Clients.VideoManager.DependencyInjection;
using VideoProcessor.Data.S3.DependencyInjection;
using VideoProcessor.FFMPEG.DependencyInjection;
using VideoProcessor.Worker.Consumers;
using VideoProcessor.Worker.Contracts;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddS3FileManager(builder.Configuration);
builder.Services.AddFFMEGVideoProcessingLibrary();
builder.Services.AddVideoManagerClient(builder.Configuration);
builder.Services.AddUseCases();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<VideosToProcessConsumer>();

    x.UsingAmazonSqs((context, cfg) =>
    {
        // TODO: Update to real AWS instead of local stack
        cfg.Host(new Uri("amazonsqs://localhost:4566"), h =>
        {
            h.AccessKey("admin");
            h.SecretKey("admin");

            h.Config(new AmazonSQSConfig { ServiceURL = "http://localhost:4566" });
            h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = "http://localhost:4566" });
        });

        cfg.ReceiveEndpoint("videos-to-process", e =>
        {
            e.ConcurrentMessageLimit = 2;
            e.ConfigureConsumer<VideosToProcessConsumer>(context);
        });
    });
});

var host = builder.Build();

var bus = host.Services.GetRequiredService<IBus>();
var message = new VideoToProcessMessage()
{
    UserEmail = "andre@email.com",
    VideoId = "Marvel_DOTNET_CSHARP.mp4"
};

await bus.Publish(message);

host.Run();