
using VideoProcessor.Domain.Entities;

namespace VideoProcessor.Domain.Ports
{
    public interface IVideoProcessingLibrary
    {
        Task<IEnumerable<ImageFile>> ExtractImagesAsync(VideoFile videoFile);
    }
}
