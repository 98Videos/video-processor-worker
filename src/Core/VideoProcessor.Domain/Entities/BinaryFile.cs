namespace VideoProcessor.Domain.Entities
{
    public record BinaryFile
    {
        public required string Identifier { get; set; }
        public required byte[] File { get; set; }
    }
}