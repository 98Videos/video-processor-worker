namespace VideoProcessor.Domain.Entities
{
    public record ProcessFile
    {
        public ProcessFile(string identifier, Stream fileStreamReference)
        {
            Identifier = identifier;
            FileStreamReference = fileStreamReference;
        }

        public string Identifier { get; private set; }

        public Stream FileStreamReference { get; private set; }
    }
}