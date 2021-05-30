using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Lib.Abstracts;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Mix.Lib.Services
{
    public class TranslatorService : JsonConfigurationServiceBase
    {
        public TranslatorService() : base(MixAppConfigFilePath.Translator)
        {
        }
        public T Translate<T>(string name, string culture, T defaultVaule = default)
        {
            return GetConfig(culture, name, defaultVaule);
        }

        public string TranslateString(string culture, string name, string defaultValue = null)
        {
            return GetConfig(culture, name) ?? defaultValue;
        }

        public void Reload(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction
                , out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var translator = new JObject();
                var ListLanguage = context.MixLanguage.ToList();
                var cultures = context.MixCulture.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new JObject();
                    foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new JProperty(lang.Keyword, lang.Value ?? lang.DefaultValue);
                        arr.Add(l);
                    }
                    translator.Add(new JProperty(culture.Specificulture, arr));
                }
                AppSettings = translator;
                SaveSettings();
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(true, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixLanguage>(ex, isRoot, transaction);
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }


    }
}
