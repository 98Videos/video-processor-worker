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
            using (var zipFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Update, true))
                {
                    foreach (var imageFile in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(imageFile.Identifier);
                        using var zipFileEntryStream = zipEntry.Open();
                        using var imageFileStream = new MemoryStream(imageFile.Content);

                        imageFileStream.CopyTo(zipFileEntryStream);
                    }
                };

                var originalVideoIdentifier = $"{files.First().OriginalVideoIdentifier}_thumbs";

                return new ZipFile($"{originalVideoIdentifier}.zip", zipFileStream.ToArray());
            }
        }
    }
}