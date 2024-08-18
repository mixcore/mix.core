using Microsoft.AspNetCore.Http;
using Mix.Communicator.Models;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Services
{
    public class MixEdmService : TenantServiceBase, IMixEdmService
    {
        protected readonly IMemoryQueueService<MessageQueueModel> _queueService;
        protected readonly UnitOfWorkInfo<MixCmsContext> _uow;

        public MixEdmService(
            IHttpContextAccessor httpContextAccessor,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixCacheService cacheService = null,
            IMixTenantService mixTenantService = null,
            UnitOfWorkInfo<MixCmsContext> uow = null) : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _queueService = queueService;
        }

        public async Task<string> GetEdmTemplate(string filename)
        {
            if (_uow == null )
            {
                return string.Empty;
            }

            var edmTemplate = await MixTemplateViewModel.GetRepository(_uow, CacheService).GetSingleAsync(
               m => m.FolderType == MixTemplateFolderType.Edms
                        && m.FileName == filename);
            return edmTemplate?.Content;
        }

        public virtual async Task SendMailWithEdmTemplate(
            string subject, string templateName, JObject data,
            string to,
            string cc = null,
            string from = null,
            string fromName = null)
        {
            var template = await GetEdmTemplate(templateName);
            if (string.IsNullOrEmpty(template))
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Edm {templateName} not found");
            }

            foreach (var prop in data.Properties().ToList())
            {
                if (data.ContainsKey(prop.Name))
                {
                    template = template.Replace($"[[{prop.Name.ToTitleCase()}]]", data.GetValue(prop.Name).ToString());
                }
            }

            EmailMessageModel msg = new()
            {
                Subject = subject,
                Message = template,
                From = from,
                FromName = fromName,
                CC = cc,
                To = to
            };
            _queueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.SendMail, msg);
        }
        
        public virtual async Task SendMailWithTemplate(
            string subject, string template, JObject data,
            string to,
            string cc = null,
            string from = null,
            string fromName = null)
        {
            foreach (var prop in data.Properties().ToList())
            {
                if (data.ContainsKey(prop.Name))
                {
                    template = template.Replace($"[[{prop.Name}]]", data.GetValue(prop.Name).ToString());
                }
            }

            EmailMessageModel msg = new()
            {
                Subject = subject,
                Message = template,
                From = from,
                FromName = fromName,
                CC = cc,
                To = to
            };
            _queueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.SendMail, msg);
        }
    }
}
