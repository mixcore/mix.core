using Mix.Portal.Domain.Models;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixModuleContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixModuleContent, int, MixModuleContentViewModel>
    {
        #region Contructors

        public MixModuleContentViewModel()
        {
        }

        public MixModuleContentViewModel(MixModuleContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public MixModuleContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType? Type { get; set; }
        public string SimpleDataColumns { get; set; }

        public List<ModuleColumnModel> Columns { get; set; }

        #endregion

        public override Task<MixModuleContent> ParseEntity(MixCacheService cacheService = null)
        {
            var arrField = Columns != null ? JArray.Parse(
               JsonConvert.SerializeObject(Columns.OrderBy(c => c.Priority).Where(
                   c => !string.IsNullOrEmpty(c.SystemName)))) : new JArray();
            SimpleDataColumns = arrField.ToString(Formatting.None);
            return base.ParseEntity(cacheService);
        }

        public override Task ExpandView(MixCacheService cacheService = null)
        {
            if (!string.IsNullOrEmpty(SimpleDataColumns))
            {
                JArray arrField = JArray.Parse(SimpleDataColumns);
                Columns = arrField.ToObject<List<ModuleColumnModel>>();
            }
            else
            {
                Columns = new List<ModuleColumnModel>();
            }

            return base.ExpandView(cacheService);
        }

        public override async Task<int> CreateParentAsync()
        {
            MixModuleViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                SystemName = SystemName,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            if (Repository.GetListQuery(m => m.ParentId == ParentId).Count() == 1)
            {
                var mdlRepo = MixModuleViewModel.GetRepository(UowInfo);

                await Repository.DeleteAsync(Id);
                await mdlRepo.DeleteAsync(ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
