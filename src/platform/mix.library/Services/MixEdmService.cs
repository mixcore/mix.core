using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Communicator.Services;
using MySqlX.XDevAPI.Relational;

namespace Mix.Lib.Services
{
    public sealed class MixEdmService : TenantServiceBase
    {
        private readonly EmailService _emailService;
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        public MixEdmService(
            IHttpContextAccessor httpContextAccessor,
            EmailService emailService,
            UnitOfWorkInfo<MixCmsContext> uow) : base(httpContextAccessor)
        {
            _emailService = emailService;
            _uow = uow;
        }

        public async Task<string?> GetEdmTemplate(string filename)
        {
            var edmTemplate = await MixTemplateViewModel.GetRepository(_uow).GetSingleAsync(
               m => m.MixTenantId== CurrentTenant.Id 
                        && m.FolderType == MixTemplateFolderType.Edms 
                        && m.FileName == filename);
            return edmTemplate?.Content;
        }

        public Task SendMailWithEdmTemplate(string subject, string template, JObject data, string to, string? from = null)
        {
            return Task.Run(() =>
            {
                foreach (var prop in data.Properties())
                {
                    template = template.Replace($"[[{prop.Name.ToTitleCase()}]]", data.Value<string>(prop.Name));
                }
                _emailService.SendMail(subject, template, to, from);
            });
        }
    }
}
