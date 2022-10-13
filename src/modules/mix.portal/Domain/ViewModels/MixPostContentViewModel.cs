namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixPostContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase<MixCmsContext, MixPostContent, int, MixPostContentViewModel>
    {
        #region Constructors

        public MixPostContentViewModel()
        {
        }

        public MixPostContentViewModel(MixPostContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public new string MixDatabaseName { get; set; } = MixDatabaseNames.POST_COLUMN;
        public string ClassName { get; set; }
        public string DetailUrl { get; set; }

        public List<MixUrlAliasViewModel> UrlAliases { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            MixDatabaseName ??= MixDatabaseNames.POST_COLUMN;
            await LoadAliasAsync();
            await base.ExpandView();
        }

        public override async Task<int> CreateParentAsync()
        {
            MixPostViewModel parent = new()
            {
                DisplayName = Title,
                Description = Excerpt,
                MixTenantId = MixTenantId
            };
            parent.SetUowInfo(UowInfo);
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            Context.MixPagePostAssociation.RemoveRange(Context.MixPagePostAssociation.Where(m => m.ChildId == Id));
            Context.MixModulePostAssociation.RemoveRange(Context.MixModulePostAssociation.Where(m => m.ChildId == Id));
            Context.MixDataContentAssociation.RemoveRange(Context.MixDataContentAssociation.Where(m => m.ParentType == MixDatabaseParentType.Post && m.IntParentId == Id));

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

        public async Task LoadContributorsAsync(MixIdentityService identityService)
        {
            Contributors = await MixContributorViewModel.GetRepository(UowInfo).GetAllAsync(
                m => m.ContentType == MixContentType.Post && m.IntContentId == Id);
            foreach (var item in Contributors)
            {
                await item.LoadUserDataAsync(identityService);
            }
        }

        private async Task LoadAliasAsync()
        {
            var aliasRepo = MixUrlAliasViewModel.GetRepository(UowInfo);
            UrlAliases = await aliasRepo.GetListAsync(
                m => m.Type == MixUrlAliasType.Post && m.SourceContentId == Id);
            DetailUrl = UrlAliases.Count > 0 ? UrlAliases[0].Alias
                : $"/post/{Id}/{SeoName}";
        }
        #endregion
    }
}
