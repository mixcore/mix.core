using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Services
{
    public sealed class MixConfigurationService : TenantServiceBase
    {
        public List<MixConfigurationContentViewModel> Configs { get; private set; }

        public MixConfigurationService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public async Task Reload(UnitOfWorkInfo<MixCmsContext> uow)
        {
            if (GlobalConfigService.Instance.InitStatus != InitStep.Blank)
            {
                Configs = await MixConfigurationContentViewModel.GetRepository(uow).GetAllAsync(m => m.MixTenantId == CurrentTenant.Id);
            }
        }

        public async Task CreateConfiguration(string name, string content, string culture, int cultureId, UnitOfWorkInfo<MixCmsContext> uow)
        {
            MixConfigurationContentViewModel config = new MixConfigurationContentViewModel(uow)
            {
                DisplayName = name,
                SystemName = SeoHelper.GetSEOString(name),
                Content = content,
                Specificulture = culture,
                MixCultureId = cultureId
            };
            await config.SaveAsync();
        }

        public T GetConfig<T>(string name, string culture, T defaultValue = default)
        {
            var config = Configs?.FirstOrDefault(m => m.Specificulture == culture && m.SystemName == name);
            return config != null ? config.GetValue<T>() : defaultValue;
        }
    }
}
