namespace VideoProcessor.Domain.Entities
{
    public record ImageFile : ProcessFile
    {
        public ImageFile(string identifier, byte[] content, string originalVideoIdentifier) : base(identifier, content)
        {
            OriginalVideoIdentifier = originalVideoIdentifier;
        }

        public string OriginalVideoIdentifier { get; private set; }

    }
}