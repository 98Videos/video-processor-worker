namespace VideoProcessor.Domain.Entities
{
    public record VideoFile : ProcessFile
    {
        public VideoFile(string identifier, byte[] content) : base(identifier, content)
        {
        }
    }
}