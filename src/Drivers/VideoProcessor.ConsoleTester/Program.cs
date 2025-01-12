using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoProcessor.Application.DependencyInjection;
using VideoProcessor.Application.UseCases;
using VideoProcessor.Clients.VideoManager.DependencyInjection;
using VideoProcessor.Data.S3.Adapters;
using VideoProcessor.Data.S3.DependencyInjection;
using VideoProcessor.FFMPEG.Adapters;
using VideoProcessor.FFMPEG.DependencyInjection;

Console.WriteLine("Running program...");

var sc = new ServiceCollection();
var loggerFactory = LoggerFactory.Create(x =>
{
});

var loggerFFMPeg = loggerFactory.CreateLogger<FFMPEGVideoProcessingLibrary>();
var loggerUseCase = loggerFactory.CreateLogger<ExtractVideoImagesToZipUseCase>();
var loggerS3 = loggerFactory.CreateLogger<S3FileRepository>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

sc.AddSingleton(loggerFFMPeg);
sc.AddSingleton(loggerUseCase);
sc.AddSingleton(loggerS3);

sc.AddS3FileManager(config);
sc.AddFFMEGVideoProcessingLibrary();
sc.AddVideoManagerClient(config);
sc.AddUseCases();

var sp = sc.BuildServiceProvider();

var useCase = sp.GetRequiredService<ExtractVideoImagesToZipUseCase>();

const string videoIdentifier = "Marvel_DOTNET_CSHARP.mp4";
const string userEmail = "andre@email.com";

await useCase.Execute(videoIdentifier, userEmail);

Console.WriteLine("Done!");