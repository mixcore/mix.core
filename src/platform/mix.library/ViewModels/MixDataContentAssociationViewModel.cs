using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels
{
    public class MixDataContentAssociationViewModel
        : MultilanguageContentViewModelBase<MixCmsContext, MixDataContentAssociation, Guid>
    {
        #region Contructors

        public MixDataContentAssociationViewModel()
        {
        }

        public MixDataContentAssociationViewModel(Repository<MixCmsContext, MixDataContentAssociation, Guid> repository) 
            : base(repository)
        {
        }

        public MixDataContentAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentAssociationViewModel(MixDataContentAssociation entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
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
