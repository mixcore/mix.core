namespace Mix.Storage.Lib.Engines.Aws
{
    public class AwsSetting
    {
        public string BucketName { get; set; }

        public string CloudFrontUrl { get; set; }

        public string BucketUrl { get; set; }

        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string Region { get; set; }
    }
}
