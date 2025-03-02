using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class TemplateHandler
    {
        public static Task MessageQueueHandler(MessageQueueModel data)
        {
            var template = data.ParseData<MixTemplateViewModel>();
            switch (data.Action)
            {
                case "Get":
                case "PATCH":
                    break;
                case "Post":
                case "Put":
                    SaveTemplate(template);
                    break;
                case "DELETE":
                    DeleteTemplate(template);
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
        private static void DeleteTemplate(MixTemplateViewModel template)
        {
            MixFileHelper.DeleteFile($"{template.FileFolder}/{template.FileName}{template.Extension}");
        }

        private static void SaveTemplate(MixTemplateViewModel template)
        {
            MixFileHelper.SaveFile(new FileModel()
            {
                Content = template.Content,
                Filename = template.FileName,
                Extension = template.Extension,
                FileFolder = template.FileFolder
            });
        }
    }
}
