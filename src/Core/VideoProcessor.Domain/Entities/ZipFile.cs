using System.IO.Compression;

namespace VideoProcessor.Domain.Entities
{
    public record ZipFile : ProcessFile
    {
        private ZipFile(string identifier, Stream fileStreamReference) : base(identifier, fileStreamReference)
        {
        }

        public static ZipFile Create(IEnumerable<ImageFile> files)
        {
            var zipFileStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Update, true))
            {
                foreach (var imageFile in files)
                {
                    var zipEntry = zipArchive.CreateEntry(imageFile.Identifier);
                    using var zipFileEntryStream = zipEntry.Open();

                    imageFile.FileStreamReference.CopyTo(zipFileEntryStream);
                    imageFile.FileStreamReference.Dispose();
                }
            };

            var originalVideoIdentifier = $"{files.First().OriginalVideoIdentifier}_thumbs";

            return new ZipFile($"{originalVideoIdentifier}.zip", zipFileStream);
        }
    }
}