using Mix.Lib.ViewModels;

namespace Mixcore.Domain.ViewModels
{
    public class NavigationViewModel 
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, NavigationViewModel>
    {
        #region Contructors

        public NavigationViewModel()
        {
        }

        public NavigationViewModel(MixDataContent entity) : base(entity)
        {
        }

        public NavigationViewModel(UnitOfWorkInfo unitOfWorkInfo = null) : base(unitOfWorkInfo)
        {
        }

        public NavigationViewModel(MixDataContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Data { get; set; }

        public List<MixDataContentViewModel> ChildData { get; set; } = new();
        public List<MixDataContentAssociationViewModel> RelatedData { get; set; } = new();

        public Guid? ContentGuidParentId { get; set; }
        public int? ContentIntParentId { get; set; }
        public MixDatabaseParentType ContentParentType { get; set; }


        public MixNavigation Nav
        {
            get
            {
                if (MixDatabaseName == MixDatabaseNames.NAVIGATION && Data != null)
                {
                    return new MixNavigation(Data, Specificulture);
                }
                return null;
            }
        }

        #endregion
    }
}
