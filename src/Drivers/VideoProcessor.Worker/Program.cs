using Serilog;
using VideoProcessor.Application.DependencyInjection;
using VideoProcessor.Clients.VideoManager.DependencyInjection;
using VideoProcessor.Data.S3.DependencyInjection;
using VideoProcessor.FFMPEG.DependencyInjection;
using VideoProcessor.Worker.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder();

var config = builder.Configuration;
var services = builder.Services;

builder.WebHost.UseKestrelHttpsConfiguration();

services.AddHealthChecks();

services
    .ConfigureLogging()
    .AddSQSMessageConsumer(config)
    .AddS3FileManager(config)
    .AddFFMEGVideoProcessingLibrary()
    .AddVideoManagerClient(config)
    .AddUseCases();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");

app.Run();