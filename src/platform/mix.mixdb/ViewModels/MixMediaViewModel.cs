using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixDbMediaViewModel : ViewModelBase<MixDbDbContext, MixDbMedia, Guid, MixDbMediaViewModel>
    {
        #region Properties
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FileProperties { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Source { get; set; }
        public string TargetUrl { get; set; }
        #endregion

        #region Constructors
        public MixDbMediaViewModel()
        {
        }

        public MixDbMediaViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixDbMediaViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbMediaViewModel(MixDbMedia entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        #endregion
    }
}
