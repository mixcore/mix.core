using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataContentAssociationViewModel 
        : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid>
    {
        #region Contructors

        public MixDataContentAssociationViewModel()
        {
        }

        public MixDataContentAssociationViewModel(Repository<MixCmsContext, MixDataContentAssociation, Guid> repository) : base(repository)
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
        public Guid GuidParentId { get; set; }
        public int IntParentId { get; set; }

        public MixDataContentViewModel DataContent{ get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            using var valRepo = new QueryRepository<MixCmsContext, MixDataContent, Guid>(UowInfo);
            DataContent = await valRepo.GetSingleViewAsync<MixDataContentViewModel>(m => m.MixDataContentId == DataContentId);
        }

        #endregion

    }
}
