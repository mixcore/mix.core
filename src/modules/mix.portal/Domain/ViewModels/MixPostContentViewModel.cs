namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixPostContent, int, MixPostContentViewModel>
    {
        #region Contructors

        public MixPostContentViewModel()
        {
        }

        public MixPostContentViewModel(MixPostContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        #endregion

        #region Overrides

        public override Task ExpandView()
        {
            MixDatabaseName ??= MixDatabaseNames.POST_COLUMN;
            return base.ExpandView();
        }


        public override async Task<int> CreateParentAsync()
        {
            MixPostViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            if (Repository.GetListQuery(m => m.ParentId == ParentId).Count() == 1)
            {
                var postRepo = MixPostViewModel.GetRepository(UowInfo);
                await Repository.DeleteAsync(Id);
                await postRepo.DeleteAsync(ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
        #endregion
    }
}
