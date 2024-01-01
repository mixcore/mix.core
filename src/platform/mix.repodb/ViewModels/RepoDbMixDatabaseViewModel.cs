using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Base;
using System.ComponentModel.DataAnnotations;

namespace Mix.RepoDb.ViewModels
{
    public sealed class RepoDbMixDatabaseViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, RepoDbMixDatabaseViewModel>
    {
        #region Properties
        public int? MixDatabaseContextId { get; set; }
        [Required]
        public string SystemName { get; set; }

        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public List<MixDatabaseColumnViewModel> Columns { get; set; } = new();
        public List<MixDatabaseRelationshipViewModel> Relationships { get; set; } = new();
        public MixDatabaseContextReadViewModel MixDatabaseContext { get; set; }
        #endregion

        #region Constructors

        public RepoDbMixDatabaseViewModel()
        {

        }

        public RepoDbMixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public RepoDbMixDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo? uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Columns = await MixDatabaseColumnViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.MixDatabaseId == Id, cancellationToken);
            Relationships = await MixDatabaseRelationshipViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.ParentId == Id, cancellationToken);
            if (MixDatabaseContextId.HasValue)
            {
                MixDatabaseContext = await MixDatabaseContextReadViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == MixDatabaseContextId.Value);
            }
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity, CancellationToken cancellationToken = default)
        {
            if (Columns != null)
            {
                if ((Type == MixDatabaseType.AdditionalData || Type == MixDatabaseType.GuidAdditionalData))
                {
                    if (!Columns.Any(m => m.SystemName == "parentId"))
                    {

                        Columns.Add(new()
                        {
                            DisplayName = "Parent Id",
                            SystemName = "parentId",
                            DataType = Type == MixDatabaseType.AdditionalData ? MixDataType.Reference : MixDataType.Guid
                        });
                    }
                    if (!Columns.Any(m => m.SystemName == "parentType"))
                    {
                        Columns.Add(new()
                        {
                            DisplayName = "Parent Type",
                            SystemName = "parentType",
                            DataType = MixDataType.Text,
                            ColumnConfigurations = new()
                            {
                                MaxLength = 20
                            }
                        });
                    }
                }

                foreach (var item in Columns)
                {
                    item.SetUowInfo(UowInfo, CacheService);
                    item.MixDatabaseId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }

            if (Relationships != null)
            {
                foreach (var item in Relationships)
                {
                    item.SetUowInfo(UowInfo, CacheService);
                    item.ParentId = parentEntity.Id;
                    item.SourceDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            // Exception: This MySqlConnection is already in use. See https://fl.vu/mysql-conn-reuse when delete nested entity using Repository
            //await MixDataContentValueViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            //await MixDataViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            //await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            foreach (var col in Columns)
            {
                col.SetUowInfo(UowInfo, CacheService);
                await col.DeleteAsync(cancellationToken);
            }

            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion
    }
}
