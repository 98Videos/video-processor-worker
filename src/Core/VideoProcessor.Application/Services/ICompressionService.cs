using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Application.Services
{
    public interface ICompressionService
    {
        BinaryFile CreateZipFile(IEnumerable<BinaryFile> files, string outputZipFileName)
    }
}