using Mix.Lib.Services;

namespace Mix.Lib.ViewModels
{
    public sealed class MixContributorViewModel
        : ViewModelBase<MixCmsContext, MixContributor, int, MixContributorViewModel>
    {
        #region Properties
        public int MixTenantId { get; set; }
        public Guid UserId { get; set; }
        public bool IsOwner { get; set; }
        public int? IntContentId { get; set; }
        public Guid? GuidContentId { get; set; }
        public MixContentType ContentType { get; set; }

        public string Avatar { get; set; }
        public string UserName { get; set; }
        #endregion

        #region Constructors

        public MixContributorViewModel()
        {
        }

        public MixContributorViewModel(MixContributor entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        public MixContributorViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion

        #region Expands

        public async Task LoadUserDataAsync(MixIdentityService identityService)
        {
            var userData = await identityService.GetUserAsync(UserId);
            if (userData != null)
            {
                UserName = userData.UserName;
                Avatar = userData.Avatar ?? MixConstants.CONST_DEFAULT_AVATAR;
            }
        }
        #endregion
    }
}
