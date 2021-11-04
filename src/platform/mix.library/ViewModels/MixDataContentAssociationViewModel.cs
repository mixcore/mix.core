using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels
{
    public class MixDataContentAssociationViewModel
        : MultilanguageContentViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, MixDataContentAssociationViewModel>
    {
        #region Contructors

        public MixDataContentAssociationViewModel()
        {
        }

        public MixDataContentAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentAssociationViewModel(MixDataContentAssociation entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }
        #endregion

        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public Guid DataContentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }

        public MixDataContentViewModel ChildDataContent { get; set; }

        #endregion

        #region Overrides


        #endregion

    }
}
