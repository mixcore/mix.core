using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Lib.Services
{
    public class TranslatorService : GlobalSettingServiceBase
    {
        public TranslatorService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }

        public static void Reload(MixCmsContext context = null, IDbContextTransaction transaction = null)
        {
            //UnitOfWorkHelper<MixCmsContext>.InitTransaction(context, transaction
            //    , out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            //try
            //{
            //    var translator = new JObject();
            //    var ListLanguage = context.MixLanguageContent.ToList();
            //    var cultures = context.MixCulture.ToList();
            //    foreach (var culture in cultures)
            //    {
            //        JObject arr = new();
            //        foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture).ToList())
            //        {
            //            JProperty l = new(lang.SystemName, lang.Content ?? lang.DefaultContent);
            //            arr.Add(l);
            //        }
            //        translator.Add(new JProperty(culture.Specificulture, arr));
            //    }
            //    AppSettingsModel = translator;
            //    SaveSettings();
            //    UnitOfWorkHelper<MixCmsContext>.HandleTransaction(true, isRoot, transaction);
            //}
            //catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            //{
            //    var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixLanguage>(ex, isRoot, transaction);
            //}
            //finally
            //{
            //    //if current Context is Root
            //    if (isRoot)
            //    {
            //        UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
            //    }
            //}
        }
    }
}
