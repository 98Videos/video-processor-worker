#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace VideoProcessor.Data.S3.Options
{
    public class S3BucketOptions
    {
        public string VideosBucket { get; set; }
        public string ZipFilesBucket { get; set; }
    }
}
