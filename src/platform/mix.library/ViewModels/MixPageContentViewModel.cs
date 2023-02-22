
namespace Mix.Lib.ViewModels
{
    public sealed class MixPageContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase<MixCmsContext, MixPageContent, int, MixPageContentViewModel>
    {
        #region Constructors

        public MixPageContentViewModel()
        {
        }

        public MixPageContentViewModel(MixPageContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public new string MixDatabaseName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }
        public string DetailUrl { get; set; }

        public List<MixUrlAliasViewModel> UrlAliases { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await LoadAliasAsync(cancellationToken);
            await base.ExpandView(cancellationToken);
        }

        private async Task LoadAliasAsync(CancellationToken cancellationToken = default)
        {
            var aliasRepo = MixUrlAliasViewModel.GetRepository(UowInfo);
            UrlAliases = await aliasRepo.GetListAsync(m => m.Type == MixUrlAliasType.Page && m.SourceContentId == Id, cancellationToken);
            DetailUrl = UrlAliases.Count > 0 ? UrlAliases[0].Alias : $"/page/{Id}";
        }

        public override async Task<int> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            MixPageViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                Description = Excerpt,
                MixTenantId = MixTenantId
            };
            return await parent.SaveAsync(cancellationToken);
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            Context.MixPageModuleAssociation.RemoveRange(Context.MixPageModuleAssociation.Where(m => m.ParentId == Id));
            Context.MixPagePostAssociation.RemoveRange(Context.MixPagePostAssociation.Where(m => m.ParentId == Id));

            if (Repository.GetListQuery(m => m.ParentId == ParentId, cancellationToken).Count() == 1)
            {
                var pageRepo = MixPageViewModel.GetRepository(UowInfo);

                await Repository.DeleteAsync(Id, cancellationToken);
                await pageRepo.DeleteAsync(ParentId, cancellationToken);
            }
            else
            {
                await base.DeleteHandlerAsync(cancellationToken);
            }

        }

        protected override async Task SaveEntityRelationshipAsync(MixPageContent parentEntity, CancellationToken cancellationToken = default)
        {
            await SaveAliasAsync(parentEntity, cancellationToken);
        }

        private async Task SaveAliasAsync(MixPageContent parentEntity, CancellationToken cancellationToken)
        {
            if (UrlAliases != null)
            {
                foreach (var item in UrlAliases)
                {
                    item.SetUowInfo(UowInfo);
                    item.MixTenantId = MixTenantId;
                    item.SourceContentId = Id;
                    item.Type = MixUrlAliasType.Page;
                    await item.SaveAsync(cancellationToken);
                }
            }
        }
        #endregion
    }
}
