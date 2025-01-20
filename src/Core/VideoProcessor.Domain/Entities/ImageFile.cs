namespace VideoProcessor.Domain.Entities
{
    public record ImageFile : ProcessFile
    {
        public ImageFile(string identifier, Stream fileStreamRef, string originalVideoIdentifier) : base(identifier, fileStreamRef)
        {
            OriginalVideoIdentifier = originalVideoIdentifier;
        }

        public string OriginalVideoIdentifier { get; private set; }
    }
}