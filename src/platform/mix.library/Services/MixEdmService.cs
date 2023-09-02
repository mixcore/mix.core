using Microsoft.AspNetCore.Http;
using Mix.Communicator.Models;
using Mix.Lib.Interfaces;

namespace Mix.Lib.Services
{
    public class MixEdmService : TenantServiceBase, IMixEdmService
    {
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected readonly UnitOfWorkInfo<MixCmsContext> _uow;

        public MixEdmService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService) : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _queueService = queueService;
        }

        public async Task<string> GetEdmTemplate(string filename)
        {
            var edmTemplate = await MixTemplateViewModel.GetRepository(_uow, CacheService).GetSingleAsync(
               m => m.FolderType == MixTemplateFolderType.Edms
                        && m.FileName == filename);
            return edmTemplate?.Content;
        }

        public virtual async Task SendMailWithEdmTemplate(
            string subject, string templateName, JObject data, 
            string to, 
            string cc = null,
            string from = null)
        {
            var template = await GetEdmTemplate(templateName);
            if (template == null)
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
                CC = cc,
                To = to
            };
            _queueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.SendMail, msg);
        }
    }
}
