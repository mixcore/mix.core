namespace Mix.Storage.Lib.ViewModels
{
    public class MixMediaViewModel : TenantDataViewModelBase<MixCmsContext, MixMedia, Guid, MixMediaViewModel>
    {
        #region Constructors

        public MixMediaViewModel()
        {
        }

        public MixMediaViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMediaViewModel(MixMedia entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

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

        public string FullPath => $"{FileFolder}/{FileName}{Extension}";
        #endregion
    }
}
