using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, MixDatabaseViewModel>
    {
        #region Properties
        [Required]
        public virtual string SystemName { get; set; }

        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;

        public List<MixDatabaseColumnViewModel> Columns { get; set; } = new();
        public List<MixDatabaseRelationshipViewModel> Relationships { get; set; } = new();
        #endregion

        #region Constructors

        public MixDatabaseViewModel()
        {

        }

        public MixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Columns = await colRepo.GetListAsync(c => c.MixDatabaseId == Id);

            var relationshipRepo = MixDatabaseRelationshipViewModel.GetRepository(UowInfo);
            Relationships = await relationshipRepo.GetListAsync(c => c.LeftId == Id || c.RightId == Id);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity)
        {
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    item.SetUowInfo(UowInfo);
                    item.MixDatabaseId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync();
                }
            }

            if (Relationships != null)
            {
                foreach (var item in Relationships)
                {
                    item.SetUowInfo(UowInfo);
                    item.MixTenantId = MixTenantId;
                    item.LeftId = parentEntity.Id;
                    await item.SaveAsync();
                }
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            await MixDataContentValueViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await MixDataViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await base.DeleteHandlerAsync();
        }

        #endregion
    }
}
