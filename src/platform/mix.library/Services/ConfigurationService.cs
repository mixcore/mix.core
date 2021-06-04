using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Lib.Abstracts;
using Mix.Shared.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Database.Entities.Cms.v2;

namespace Mix.Lib.Services
{
    public class ConfigurationService : JsonConfigurationServiceBase
    {
        public ConfigurationService() : base(MixAppConfigFilePath.Configration)
        {
        }

        public static void Reload(MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction
                , out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var translator = new JObject();
                var lstConfig = context.MixConfigurationContent.ToList();
                var cultures = context.MixCulture.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new();
                    foreach (var conf in lstConfig.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new(conf.SystemName, conf.Content);
                        arr.Add(l);
                    }
                    translator.Add(new JProperty(culture.Specificulture, arr));
                }
                AppSettings = translator;
                SaveSettings();
                UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(true, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContextV2>.HandleException<MixLanguage>(ex, isRoot, transaction);
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContextV2>.CloseDbContext(ref context, ref transaction);
                }
            }
        }


    }
}
