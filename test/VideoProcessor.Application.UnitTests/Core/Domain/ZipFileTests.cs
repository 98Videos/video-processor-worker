using AutoBogus;
using FluentAssertions;
using System.IO.Compression;
using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Application.UnitTests.Core.Domain
{
    public class ZipFileTests
    {
        [Test]
        public void Create_WhenReceivingImageFileEnumerable_ShouldReturnZipFileWithEntries()
        {
            // Arrange
            const string originalVideoIdentifier = "testvideo";
            var imageFiles = new AutoFaker<ImageFile>()
                .RuleFor(x => x.OriginalVideoIdentifier, originalVideoIdentifier)
                .Generate(4);

            // Act
            var zipFile = VideoProcessor.Domain.Entities.ZipFile.Create(imageFiles);

            // Assert
            zipFile.Identifier.Should().Be($"{originalVideoIdentifier}_thumbs.zip");

            using var fileStream = new MemoryStream(zipFile.Content);
            using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read);

            zipArchive.Entries.Count.Should().Be(imageFiles.Count);
            zipArchive.Entries
                .Select(x => x.Name)
                .Should()
                .BeEquivalentTo(imageFiles.Select(x => x.Identifier));
        }
    }
}