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
    public class TranslatorService : JsonConfigurationServiceBase
    {
        public TranslatorService() : base(MixAppConfigFilePath.Translator)
        {
        }
        public static T Translate<T>(string name, string culture, T defaultVaule = default)
        {
            return GetConfig(culture, name, defaultVaule);
        }

        public static string TranslateString(string culture, string name, string defaultValue = null)
        {
            return GetConfig(culture, name) ?? defaultValue;
        }

        public static void Reload(MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(_context, _transaction
                , out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var translator = new JObject();
                var ListLanguage = context.MixLanguageContents.ToList();
                var cultures = context.MixCultures.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new();
                    foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new(lang.SystemName, lang.Content ?? lang.DefaultContent);
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
