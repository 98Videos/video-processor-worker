using System.IO.Compression;

namespace VideoProcessor.Domain.Entities
{
    public record ZipFile : ProcessFile
    {
        private ZipFile(string identifier, byte[] content) : base(identifier, content)
        {
        }

        public static ZipFile Create(IEnumerable<ImageFile> files)
        {
            using var zipFileStream = new MemoryStream();
            var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create);

            foreach (var imageFile in files)
            {
                var zipEntry = zipArchive.CreateEntry(imageFile.Identifier);
                using var imageFileStream = new MemoryStream(imageFile.Content);
                using var zipFileEntryStream = zipEntry.Open();

                imageFileStream.CopyTo(zipFileEntryStream);
            }

            var originalVideoIdentifier = files.First().OriginalVideoIdentifier;
            return new ZipFile($"{originalVideoIdentifier}.zip", zipFileStream.ToArray());
        }
    }
}