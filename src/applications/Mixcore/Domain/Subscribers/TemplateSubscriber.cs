using Mix.Lib.Subscribers;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class TemplateSubscriber : SubscriberBase
    {
        static string topicId = typeof(MixTemplateViewModel).FullName;
        private readonly ILogger<TemplateSubscriber> logger;

        public TemplateSubscriber(
            ILogger<TemplateSubscriber> logger,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            this.logger = logger;
        }

        public override Task Handler(MessageQueueModel data)
        {
            var template = data.Model.ToObject<MixTemplateViewModel>();
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
            logger.LogWarning("Removed Template File {0}/{1}{2}", template.FileFolder, template.FileName, template.Extension);
        }

        private void SaveTemplate(MixTemplateViewModel template)
        {
            MixFileHelper.SaveFile(new Mix.Heart.Models.FileModel()
            {
                Content = template.Content,
                Filename = template.FileName,
                Extension = template.Extension,
                FileFolder = template.FileFolder
            });
            logger.LogInformation("Saved Template File {0}/{1}{2}", template.FileFolder, template.FileName, template.Extension);
        }
    }
}
