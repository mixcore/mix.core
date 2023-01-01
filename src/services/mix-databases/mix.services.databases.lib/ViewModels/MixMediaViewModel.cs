using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Helpers;
using Mix.Services.Databases.Lib.Entities;
using Mix.Shared.Services;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixMediaViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixMedia, int, MixMediaViewModel>
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

        public MixMediaViewModel(MixServiceDatabaseDbContext context) : base(context)
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
