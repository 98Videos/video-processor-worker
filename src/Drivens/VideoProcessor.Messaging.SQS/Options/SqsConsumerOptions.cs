﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace VideoProcessor.Messaging.SQS.Options
{
    internal class SqsConsumerOptions
    {
        public string QueueUrl { get; set; }
    }
}
