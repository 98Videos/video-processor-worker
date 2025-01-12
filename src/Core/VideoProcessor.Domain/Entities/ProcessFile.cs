namespace VideoProcessor.Domain.Entities
{
    public record ProcessFile
    {
        public ProcessFile(string identifier, byte[] content)
        {
            Identifier = identifier;
            Content = content;
        }

        public string Identifier { get; set; }
        public byte[] Content { get; set; }
    }
}