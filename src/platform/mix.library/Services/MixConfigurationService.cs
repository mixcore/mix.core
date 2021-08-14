using Microsoft.EntityFrameworkCore.Storage;
using Mix.Shared.Constants;
using Mix.Lib.Abstracts;
using Mix.Database.Entities.Cms;

namespace Mix.Lib.Services
{
    public class MixConfigurationService : JsonConfigurationServiceBase
    {
        public MixConfigurationService() : base(MixAppConfigFilePath.Configration)
        {
        }

        public static void Reload(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction
            //    , out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            //try
            //{
            //    var translator = new JObject();
            //    var lstConfig = context.MixConfigurationContent.ToList();
            //    var cultures = context.MixCulture.ToList();
            //    foreach (var culture in cultures)
            //    {
            //        JObject arr = new();
            //        foreach (var conf in lstConfig.Where(l => l.Specificulture == culture.Specificulture).ToList())
            //        {
            //            JProperty l = new(conf.SystemName, conf.Content);
            //            arr.Add(l);
            //        }
            //        translator.Add(new JProperty(culture.Specificulture, arr));
            //    }
            //    AppSettings = translator;
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
