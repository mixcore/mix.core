namespace Mix.Portal.Domain.ViewModels
{
    public class MixApplicationViewModel
        : TenantDataViewModelBase<MixCmsContext, MixApplication, int, MixApplicationViewModel>
    {
        #region Properties

        public string Title { get; set; }
        public string BaseHref { get; set; }
        public string BaseRoute { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        public string PackateFilePath { get; set; }
        #endregion

        #region Constructors

        public MixApplicationViewModel()
        {
        }

        public MixApplicationViewModel(MixApplication entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixApplicationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
