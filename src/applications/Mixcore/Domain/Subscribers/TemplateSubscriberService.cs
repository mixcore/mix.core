using Mix.Heart.Services;
using Mix.Lib.Subscribers;
using Mix.Lib.ViewModels;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models;
using Mix.Shared.Constants;
using System.Text.Json;

namespace Mixcore.Domain.Subscribers
{
    public class TemplateSubscriberService : SubscriberService
    {
        static string topicId = typeof(MixTemplateViewModel).FullName;
        private readonly ILogger<TemplateSubscriberService> logger;

        public TemplateSubscriberService(
            ILogger<TemplateSubscriberService> logger,
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
                case Mix.Shared.Enums.MixRestAction.Get:
                    break;
                case Mix.Shared.Enums.MixRestAction.Post:
                case Mix.Shared.Enums.MixRestAction.Put:
                    SaveTemplate(template);
                    break;
                case Mix.Shared.Enums.MixRestAction.Patch:
                    break;
                case Mix.Shared.Enums.MixRestAction.Delete:
                    DeleteTemplate(template);
                    break;
                default:
                    break;
            }
            
            return Task.CompletedTask;
        }

        private void DeleteTemplate(MixTemplateViewModel template)
        {
            MixFileService.Instance.DeleteFile($"{template.FileFolder}/{template.FileName}{template.Extension}");
            logger.LogWarning("Removed Template File {0}/{1}{2}", template.FileFolder, template.FileName, template.Extension);
        }

        private void SaveTemplate(MixTemplateViewModel template)
        {
            MixFileService.Instance.SaveFile(new Mix.Heart.Models.FileModel()
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
