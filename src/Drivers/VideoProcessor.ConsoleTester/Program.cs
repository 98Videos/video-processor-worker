using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;
using VideoProcessor.FFMPEG.Adapters;
using VideoProcessor.FFMPEG.DependencyInjection;

Console.WriteLine("Running program...");

var sc = new ServiceCollection();
var loggerFactory = LoggerFactory.Create(x =>
{
});

var logger = loggerFactory.CreateLogger<FFMPEGVideoProcessingLibrary>();

sc.AddSingleton(logger);
sc.AddFFMEGVideoProcessingLibrary();

var sp = sc.BuildServiceProvider();

var videoProcessingLibrary = sp.GetRequiredService<IVideoProcessingLibrary>();

const string filePath = "Files/Marvel_DOTNET_CSHARP.mp4";
var videoBytes = File.ReadAllBytes($"./{filePath}");

var videoFile = new BinaryFile()
{
    Identifier = Path.GetFileName(filePath),
    File = videoBytes
};

var imageFiles = await videoProcessingLibrary.ExtractImagesAsync(videoFile);

if (!Directory.Exists("./images"))
    Directory.CreateDirectory("./images");

foreach (var file in imageFiles)
{
    await File.WriteAllBytesAsync($"./images/{file.Identifier}", file.File);
}

Console.WriteLine("Done!");

