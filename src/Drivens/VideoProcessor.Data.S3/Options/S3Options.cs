#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace VideoProcessor.Data.S3.Options
{
    internal class S3Options
    {
        public string VideosBucketUrl { get; set; }
        public string ZipFilesBucketUrl { get; set; }
    }
}
