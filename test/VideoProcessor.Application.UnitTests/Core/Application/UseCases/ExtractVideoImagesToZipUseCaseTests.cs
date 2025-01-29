using AutoBogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VideoProcessor.Application.UseCases;
using VideoProcessor.Domain.Entities;
using VideoProcessor.Domain.Ports;
using VideoProcessor.Domain.ValueObjects;

namespace VideoProcessor.Application.UnitTests.Core.Application.UseCases
{
    public class ExtractVideoImagesToZipUseCaseTests
    {
        private Mock<IFileRepository> _fileRepositoryMock = new();
        private Mock<IVideoProcessingLibrary> _videoProcessingLibraryMock = new();
        private Mock<IVideoManagerClient> _videoManagerClientMock = new();
        private Mock<IEmailSender> _emailSenderMock = new();
        private Mock<ILogger<ExtractVideoImagesToZipUseCase>> _loggerMock = new();

        [SetUp]
        public void SetUp()
        {
            _fileRepositoryMock = new();
            _videoProcessingLibraryMock = new();
            _videoManagerClientMock = new();
            _emailSenderMock = new();
            _loggerMock = new();
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

            var images = new AutoFaker<ImageFile>()
                .RuleFor(x => x.FileStreamReference, f => new MemoryStream(f.Random.Bytes(5)))
                .Generate(3);

            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ReturnsAsync(images);

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _emailSenderMock.Object,
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

            var images = new AutoFaker<ImageFile>()
                .RuleFor(x => x.FileStreamReference, f => new MemoryStream(f.Random.Bytes(5)))
                .Generate(3);

            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ReturnsAsync(images);

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _emailSenderMock.Object,
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
                                                             _emailSenderMock.Object,
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

            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ThrowsAsync(new Exception("error"));

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _emailSenderMock.Object,
                                                             _loggerMock.Object);

            // Act
            await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            _videoManagerClientMock.Verify(x => x.NotifyProcessingFailure(videoIdentifier), Times.Once);
        }

        [Test]
        public async Task Execute_WhenAnExceptionIsThrown_ShouldSendAnEmailToUser()
        {
            // Arrange
            const string videoIdentifier = "videoTest.mp4";
            const string userEmail = "test@test.com";

            var videoFile = new AutoFaker<VideoFile>().Generate();

            _fileRepositoryMock
                .Setup(x => x.GetVideoFile(userEmail, videoIdentifier))
                .ReturnsAsync(videoFile);

            _videoProcessingLibraryMock
                .Setup(x => x.ExtractImagesAsync(videoFile))
                .ThrowsAsync(new Exception("error"));

            var useCase = new ExtractVideoImagesToZipUseCase(_fileRepositoryMock.Object,
                                                             _videoProcessingLibraryMock.Object,
                                                             _videoManagerClientMock.Object,
                                                             _emailSenderMock.Object,
                                                             _loggerMock.Object);

            // Act
            await useCase.Execute(videoIdentifier, userEmail);

            // Assert
            _emailSenderMock.Verify(x =>
                x.SendNotificationEmail(It.Is<NotificationEmailMessage>(email =>
                    email.To == userEmail
                    && email.Subject == "98 Videos Notification - Erro ao processar vídeo"
                    && email.Body == $"Ocorreu um erro ao processar o vídeo {videoIdentifier}. Por favor entre em contato com o suporte para mais informações."
                )),
            Times.Once);
        }
    }
}