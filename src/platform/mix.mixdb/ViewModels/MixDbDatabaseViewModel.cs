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
    public sealed class MixDbDatabaseViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, MixDbDatabaseViewModel>
    {
        #region Properties
        public int? MixDatabaseContextId { get; set; }
        [Required]
        public string SystemName { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; } = MixDatabaseNamingConvention.TitleCase;
        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public List<MixdbDatabaseColumnViewModel> Columns { get; set; } = new();
        public List<MixdbDatabaseColumnViewModel>? DefaultColumns { get; set; }
        public List<MixDatabaseRelationshipViewModel> Relationships { get; set; } = new();
        public MixDatabaseContextReadViewModel MixDatabaseContext { get; set; }

        public MixDatabaseProvider DatabaseProvider { get; set; }
        #endregion

        #region Constructors

        public MixDbDatabaseViewModel()
        {

        }

        public MixDbDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo? uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Columns = await MixdbDatabaseColumnViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.MixDatabaseId == Id, cancellationToken);
            Relationships = await MixDatabaseRelationshipViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.ParentId == Id, cancellationToken);
            if (MixDatabaseContextId.HasValue)
            {
                MixDatabaseContext = await MixDatabaseContextReadViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == MixDatabaseContextId.Value);
                NamingConvention = MixDatabaseContext.NamingConvention;
                DatabaseProvider = MixDatabaseContext.DatabaseProvider;
            }
            AddDefaultColumns();
        }

        public void AddDefaultColumns()
        {
            if (MixDatabaseContextId.HasValue)
            {

                var fieldNameSrv = new FieldNameService(NamingConvention);
                var dbConstants = MixDbHelper.GetDatabaseConstant(DatabaseProvider);
                bool isGuid = DatabaseProvider == MixDatabaseProvider.SCYLLADB || Type == MixDatabaseType.GuidService;
                if (!Columns.Any(m => m.SystemName == fieldNameSrv.Id))
                {
                    Columns.Add(new MixdbDatabaseColumnViewModel()
                    {
                        DisplayName = fieldNameSrv.Id,
                        SystemName = fieldNameSrv.Id,
                        DataType = isGuid ? MixDataType.Guid
                        : MixDataType.Integer,
                        DefaultValue = isGuid ? dbConstants.Guid : string.Empty
                    });
                }

                if (!Columns.Any(m => m.SystemName == fieldNameSrv.CreatedBy))
                {
                    Columns.Add(new MixdbDatabaseColumnViewModel()
                    {
                        DisplayName = fieldNameSrv.CreatedBy.ToSEOString(' '),
                        SystemName = fieldNameSrv.CreatedBy,
                        DataType = MixDataType.String
                    });
                }

                if (!Columns.Any(m => m.SystemName == fieldNameSrv.CreatedDateTime))
                {
                    Columns.Add(new MixdbDatabaseColumnViewModel()
                    {
                        DisplayName = fieldNameSrv.CreatedDateTime.ToSEOString(' '),
                        SystemName = fieldNameSrv.CreatedDateTime,
                        DataType = MixDataType.DateTime,
                        DefaultValue = dbConstants.Now
                    });
                    DefaultColumns = DefaultColumns?.Where(m => !Columns.Any(n => n.SystemName == m.SystemName)).ToList();
                }
            }
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity, CancellationToken cancellationToken = default)
        {
            if (Columns != null)
            {
                if (Type == MixDatabaseType.AdditionalData || Type == MixDatabaseType.GuidAdditionalData)
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
