namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public class MixPageViewModel
        : SiteDataWithContentViewModelBase
            <MixCmsContext, MixPage, int, MixPageViewModel, MixPageContent, MixPageContentViewModel>
    {
        #region Contructors

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


        public override async Task ExpandView()
        {
            var repo = MixPageContentViewModel.GetRepository(UowInfo);
            Contents = await repo.GetListAsync(m => m.ParentId == Id);
        }

        #endregion
    }
}
