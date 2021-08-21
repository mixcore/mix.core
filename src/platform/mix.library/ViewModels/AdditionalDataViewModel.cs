using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using System;

namespace Mix.Lib.ViewModels
{
    public class AdditionalDataViewModel 
        : SiteDataWithContentViewModelBase<MixCmsContext, MixData, Guid, MixDataContent, MixDataContentViewModel>
    {
        #region Contructors

        public AdditionalDataViewModel()
        {
        }

        public AdditionalDataViewModel(Repository<MixCmsContext, MixData, Guid> repository) : base(repository)
        {
        }

        public AdditionalDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataViewModel(MixData entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public AdditionalDataViewModel(
            MixDatabaseParentType parentType,
            string parentId,
            string databaseName,
            string culture = null)
        {
            using var ctx = new MixCmsContext();
            UowInfo = new(ctx);
            GlobalConfigService configSrv = new();
            culture = culture ?? configSrv.DefaultCulture;
        }
        #endregion
    }
}
