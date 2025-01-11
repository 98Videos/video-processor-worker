using System.IO.Compression;
using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Application.Services
{
    public class CompressionService : ICompressionService
    {
        public BinaryFile CreateZipFile(IEnumerable<BinaryFile> files, string outputZipFileName)
        {
            using var zipFileStream = new MemoryStream();
            var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create);

            foreach (var imageFile in files)
            {
                var zipEntry = zipArchive.CreateEntry(imageFile.Identifier);
                using var imageFileStream = new MemoryStream(imageFile.File);
                using var zipFileEntryStream = zipEntry.Open();

                imageFileStream.CopyTo(zipFileEntryStream);
            }

            return new BinaryFile()
            {
                Identifier = outputZipFileName,
                File = zipFileStream.ToArray()
            };
        }
    }
}