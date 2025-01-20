namespace VideoProcessor.Domain.Entities
{
    public record VideoFile : ProcessFile
    {
        public VideoFile(string identifier, Stream fileStreamReference) : base(identifier, fileStreamReference)
        {
        }
    }
}