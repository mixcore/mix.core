using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Mixdb.Base;
using Mix.Mixdb.Helpers;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace Mix.Mixdb.ViewModels
{
    public sealed class MixDbTableViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDbTable, int, MixDbTableViewModel>
    {
        #region Properties
        public int? MixDbDatabaseId { get; set; }
        [Required]
        public string SystemName { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; } = MixDatabaseNamingConvention.SnakeCase;
        public MixDbTableType Type { get; set; } = MixDbTableType.Service;
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public List<MixdbColumnViewModel> Columns { get; set; } = new();
        public List<MixdbColumnViewModel>? DefaultColumns { get; set; }
        public List<MixDbTableRelationshipViewModel> Relationships { get; set; } = new();
        public MixDbDatabaseReadViewModel MixDbDatabase { get; set; }

        public MixDatabaseProvider DatabaseProvider { get; set; }
        #endregion

        #region Constructors

        public MixDbTableViewModel()
        {

        }

        public MixDbTableViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbTableViewModel(MixDbTable entity, UnitOfWorkInfo? uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Columns = await MixdbColumnViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.MixDbTableId == Id, cancellationToken);
            Relationships = await MixDbTableRelationshipViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.ParentId == Id, cancellationToken);
            if (MixDbDatabaseId.HasValue)
            {
                MixDbDatabase = await MixDbDatabaseReadViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == MixDbDatabaseId.Value);
                NamingConvention = MixDbDatabase.NamingConvention;
                DatabaseProvider = MixDbDatabase.DatabaseProvider;
            }
        }

        public void AddDefaultColumns()
        {
            if (Id == 0 && MixDbDatabaseId.HasValue)
            {

                var fieldNameSrv = new FieldNameService(MixDatabaseNamingConvention.SnakeCase);
                var dbConstants = MixDbHelper.GetDatabaseConstant(DatabaseProvider);
                bool isGuid = DatabaseProvider == MixDatabaseProvider.SCYLLADB || Type == MixDbTableType.GuidService;
                if (!Columns.Any(m => m.SystemName == fieldNameSrv.Id))
                {
                    Columns.Add(new MixdbColumnViewModel()
                    {
                        DisplayName = "Id",
                        SystemName = fieldNameSrv.Id,
                        DataType = isGuid ? MixDataType.Guid
                        : MixDataType.Integer,
                        DefaultValue = isGuid ? dbConstants.Guid : string.Empty
                    });
                }

                if (!Columns.Any(m => m.SystemName == fieldNameSrv.CreatedBy))
                {
                    Columns.Add(new MixdbColumnViewModel()
                    {
                        DisplayName = "Created By",
                        SystemName = fieldNameSrv.CreatedBy,
                        DataType = MixDataType.String
                    });
                }

                if (!Columns.Any(m => m.SystemName == fieldNameSrv.CreatedDateTime))
                {
                    Columns.Add(new MixdbColumnViewModel()
                    {
                        DisplayName = "Created Date",
                        SystemName = fieldNameSrv.CreatedDateTime,
                        DataType = MixDataType.DateTime
                    });
                    DefaultColumns = DefaultColumns?.Where(m => !Columns.Any(n => n.SystemName == m.SystemName)).ToList();
                }
            }
        }

        protected override async Task SaveEntityRelationshipAsync(MixDbTable parentEntity, CancellationToken cancellationToken = default)
        {
            AddDefaultColumns();

            if (Columns != null)
            {
                if (Type == MixDbTableType.AdditionalData || Type == MixDbTableType.GuidAdditionalData)
                {
                    if (!Columns.Any(m => m.SystemName == "parentId"))
                    {

                        Columns.Add(new()
                        {
                            DisplayName = "Parent Id",
                            SystemName = "parentId",
                            DataType = Type == MixDbTableType.AdditionalData ? MixDataType.Reference : MixDataType.Guid
                        });
                    }
                    if (!Columns.Any(m => m.SystemName == "parentType"))
                    {
                        Columns.Add(new()
                        {
                            DisplayName = "Parent Type",
                            SystemName = "parentType",
                            DataType = MixDataType.String,
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
                    item.MixDbTableId = parentEntity.Id;
                    item.MixDbTableName = parentEntity.SystemName;
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
                    item.SourceTableName = parentEntity.SystemName;
                    await item.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            // Exception: This MySqlConnection is already in use. See https://fl.vu/mysql-conn-reuse when delete nested entity using Repository
            //await MixDataContentValueViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
            //await MixDataViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
            //await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
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
