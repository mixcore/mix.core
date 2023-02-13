using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixMediaViewModel : ViewModelBase<MixDbDbContext, MixMedia, int, MixMediaViewModel>
    {
        #region Properties
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? FileUrl { get; set; }
        public int MixTenantId { get; set; }
        #endregion

        #region Constructors
        public MixMediaViewModel()
        {
        }

        public MixMediaViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixMediaViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMediaViewModel(MixMedia entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        #endregion
    }
}
