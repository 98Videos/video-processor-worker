using AutoBogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VideoProcessor.Application.UseCases;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;

namespace VideoProcessor.Application.UnitTests.UseCases
{
    public class ExtractVideoImagesToZipUseCaseTests
    {
        private readonly Mock<IFileRepository> _fileRepositoryMock;
        private readonly Mock<IVideoProcessingLibrary> _videoProcessingLibraryMock;
        private readonly Mock<IVideoManagerClient> _videoManagerClientMock;
        private readonly Mock<ILogger<ExtractVideoImagesToZipUseCase>> _loggerMock;

        public ExtractVideoImagesToZipUseCaseTests()
        {
            _fileRepositoryMock = new Mock<IFileRepository>();
            _videoProcessingLibraryMock = new Mock<IVideoProcessingLibrary>();
            _videoManagerClientMock = new Mock<IVideoManagerClient>();
            _loggerMock = new Mock<ILogger<ExtractVideoImagesToZipUseCase>>();
        }

        [Test]
        public async Task Execute_WhenNoErrorOccurs_ShouldReturnSuccessResult()
        {
            // Arrange
            const string videoIdentifier = "videoTest.mp4";
            const string userEmail = "test@test.com";

            var videoFile = new AutoFaker<VideoFile>().Generate();

            _fileRepositoryMock
                .Setup(x => x.GetVideoFile(userEmail, videoIdentifier))
                .ReturnsAsync(videoFile);

            var images = new AutoFaker<ImageFile>().Generate(3);
            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ReturnsAsync(images);

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _loggerMock.Object);

            // Act
            var result = await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task Execute_WhenNoErrorOccurs_ShouldSaveZipFileAndNotifySuccess()
        {
            // Arrange
            const string videoIdentifier = "videoTest.mp4";
            const string userEmail = "test@test.com";

            var videoFile = new AutoFaker<VideoFile>().Generate();

            _fileRepositoryMock
                .Setup(x => x.GetVideoFile(userEmail, videoIdentifier))
                .ReturnsAsync(videoFile);

            var images = new AutoFaker<ImageFile>().Generate(3);
            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ReturnsAsync(images);

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _loggerMock.Object);

            // Act
            await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            _fileRepositoryMock.Verify(x => x.SaveZipFile(userEmail, It.IsAny<ZipFile>()), Times.Once());
            _videoManagerClientMock.Verify(x => x.NotifyProcessingSuccess(videoIdentifier), Times.Once);
        }

        [Test]
        public async Task Execute_AnyExceptionIsThrown_ShouldReturnFailureResult()
        {
            // Arrange
            const string videoIdentifier = "videoTest.mp4";
            const string userEmail = "test@test.com";

            var videoFile = new AutoFaker<VideoFile>().Generate();

            _fileRepositoryMock
                .Setup(x => x.GetVideoFile(userEmail, videoIdentifier))
                .ReturnsAsync(videoFile);

            var images = new AutoFaker<ImageFile>().Generate(3);
            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ThrowsAsync(new Exception("error"));

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _loggerMock.Object);

            // Act
            var result = await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            result.IsSuccessful.Should().BeFalse();
        }

        [Test]
        public async Task Execute_WhenAnExceptionIsThrown_ShouldNotifyFailure()
        {
            // Arrange
            const string videoIdentifier = "videoTest.mp4";
            const string userEmail = "test@test.com";

            var videoFile = new AutoFaker<VideoFile>().Generate();

            _fileRepositoryMock
                .Setup(x => x.GetVideoFile(userEmail, videoIdentifier))
                .ReturnsAsync(videoFile);

            var images = new AutoFaker<ImageFile>().Generate(3);
            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ThrowsAsync(new Exception("error"));

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _loggerMock.Object);

            // Act
            await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            _videoManagerClientMock.Verify(x => x.NotifyProcessingFailure(videoIdentifier), Times.Once);
        }
    }
}