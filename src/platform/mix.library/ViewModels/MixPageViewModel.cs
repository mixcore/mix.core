namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixPageViewModel
        : SiteDataWithContentViewModelBase
            <MixCmsContext, MixPage, int, MixPageViewModel, MixPageContent, MixPageContentViewModel>
    {
        #region Constructors

        public MixPageViewModel()
        {
        }

        public MixPageViewModel(MixPage entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPageViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides


        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var repo = MixPageContentViewModel.GetRepository(UowInfo);
            Contents = await repo.GetListAsync(m => m.ParentId == Id, cancellationToken);
        }

        #endregion
    }
}
