using Microsoft.EntityFrameworkCore;
using Mix.Heart.Helpers;
using Mix.Portal.Domain.ViewModels;
using Mix.RepoDb.Repositories;
using Mix.Shared.Models;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public sealed class PageContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, PageContentViewModel>
    {
        #region Constructors

        public PageContentViewModel()
        {
        }

        public PageContentViewModel(MixPageContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public string ClassName { get; set; }

        public string DetailUrl => $"/page/{Id}/{SeoName}";

        public Guid? AdditionalDataId { get; set; }

        public List<ModuleContentViewModel> Modules { get; set; }
        public PagingResponseModel<MixPagePostAssociationViewModel> Posts { get; set; }
        public JObject AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
            await LoadModulesAsync();
        }

        #endregion

        #region Public Method

        public async Task LoadPostsAsync(PagingRequestModel pagingModel)
        {
            Posts = await MixPagePostAssociationViewModel.GetRepository(UowInfo).GetPagingAsync(m => m.ParentId == Id, pagingModel);
            List<Task> tasks = new();
            foreach (var item in Posts.Items)
            {
                item.SetUowInfo(UowInfo);
                tasks.Add(item.LoadPost());
            }
            await Task.WhenAll(tasks);
        }

        public ModuleContentViewModel GetModule(string moduleName)
        {
            return Modules.FirstOrDefault(m => m.SystemName == moduleName);
        }
        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        #endregion

        #region Private Methods
        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            mixRepoDbRepository.Init(MixDatabaseName);
            var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Page, Id);
            AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        private async Task LoadModulesAsync()
        {
            var tasks = new List<Task>();
            var moduleIds = await Context.MixPageModuleAssociation
                    .AsNoTracking()
                    .Where(p => p.ParentId == Id)
                    .OrderBy(m => m.Priority)
                    .Select(m => m.ChildId)
                    .ToListAsync();
            var moduleRepo = ModuleContentViewModel.GetRepository(UowInfo);
            Modules = await moduleRepo.GetListAsync(m => moduleIds.Contains(m.Id));
            var paging = new PagingModel();
            foreach (var item in Modules)
            {
                tasks.Add(item.LoadData(paging));
            }
            await Task.WhenAll(tasks);
        }
        #endregion
    }
}
