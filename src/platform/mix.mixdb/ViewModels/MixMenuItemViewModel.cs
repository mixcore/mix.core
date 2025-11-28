using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixMenuItemViewModel : ViewModelBase<MixDbDbContext, MixMenuItem, int, MixMenuItemViewModel>
    {
        #region Properties
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public string Target { get; set; }
        public string TargetId { get; set; }
        public string Classes { get; set; }
        public string Description { get; set; }
        public string Alt { get; set; }
        public string Hreflang { get; set; }
        public string Group { get; set; }
        public string Image { get; set; }
        public int TenantId { get; set; }
        #endregion

        #region Constructors
        public MixMenuItemViewModel()
        {
        }

        public MixMenuItemViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixMenuItemViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMenuItemViewModel(MixMenuItem entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        #endregion
    }
}