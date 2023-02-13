using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class TemplateSubscriber : SubscriberBase
    {
        private static readonly string TopicId = typeof(MixTemplateViewModel).FullName;
        private readonly ILogger<TemplateSubscriber> _logger;

        public TemplateSubscriber(
            ILogger<TemplateSubscriber> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(TopicId, MixModuleNames.Mixcore, serviceProvider, configuration, queueService)
        {
            this._logger = logger;
        }

        public override Task Handler(MessageQueueModel data)
        {
            var template = data.ParseData<MixTemplateViewModel>();
            switch (data.Action)
            {
                case "Get":
                case "Patch":
                    break;
                case "Post":
                case "Put":
                    SaveTemplate(template);
                    break;
                case "Delete":
                    DeleteTemplate(template);
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }

        private void DeleteTemplate(MixTemplateViewModel template)
        {
            MixFileHelper.DeleteFile($"{template.FileFolder}/{template.FileName}{template.Extension}");
            _logger.LogWarning("Removed Template File {0}/{1}{2}", template.FileFolder, template.FileName, template.Extension);
        }

        private void SaveTemplate(MixTemplateViewModel template)
        {
            MixFileHelper.SaveFile(new FileModel()
            {
                Content = template.Content,
                Filename = template.FileName,
                Extension = template.Extension,
                FileFolder = template.FileFolder
            });
            _logger.LogInformation("Saved Template File {0}/{1}{2}", template.FileFolder, template.FileName, template.Extension);
        }
    }
}
