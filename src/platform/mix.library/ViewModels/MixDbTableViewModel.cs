using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.ViewModels.ReadOnly;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDbTableViewModel : TenantDataViewModelBase<MixCmsContext, MixDbTable, int, MixDbTableViewModel>
    {
        #region Properties
        public int? MixDbDatabaseId { get; set; }
        [Required]
        public string SystemName { get; set; }
        public MixDbTableType Type { get; set; } = MixDbTableType.Service;
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public List<MixDbColumnViewModel> Columns { get; set; } = new();
        public List<MixDbTableRelationshipViewModel> Relationships { get; set; } = new();
        public MixDbDatabaseReadViewModel MixDbDatabase { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        #endregion

        #region Constructors

        public MixDbTableViewModel()
        {
        }

        public MixDbTableViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbTableViewModel(MixDbTable entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task Validate(CancellationToken cancellationToken)
        {
            if (await Context.MixDbTable.AnyAsync(p => p.Id != Id && p.SystemName == SystemName, cancellationToken))
            {
                IsValid = false;
                Errors.Add(new ValidationResult("Database name must be unique."));
            }

            await base.Validate(cancellationToken);
        }

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var columnRepo = MixDbColumnViewModel.GetRepository(UowInfo, CacheService);
            Columns = await columnRepo.GetListAsync(c => c.MixDbTableId == Id, cancellationToken);
            Relationships = await MixDbTableRelationshipViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.ParentId == Id, cancellationToken);
            if (MixDbDatabaseId.HasValue)
            {
                MixDbDatabase = await MixDbDatabaseReadViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == MixDbDatabaseId.Value);
                NamingConvention = MixDbDatabase.NamingConvention;
            }
        }

        public override Task<MixDbTable> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (Id == default)
            {
                var fieldNameSrv = new FieldNameService(NamingConvention);
                if (!Columns.Any(m => string.Equals(m.SystemName, fieldNameSrv.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    Columns.Insert(0, new MixDbColumnViewModel()
                    {
                        DisplayName = "Id",
                        SystemName = fieldNameSrv.Id,
                        DataType = Type == MixDbTableType.GuidService ? MixDataType.Guid : MixDataType.Integer,
                        ColumnConfigurations = new Shared.Models.ColumnConfigurations()
                        {
                            IsUnique = true,
                            IsRequire = true
                        }
                    });
                }
                if (!Columns.Any(m => string.Equals(m.SystemName, fieldNameSrv.CreatedBy, StringComparison.OrdinalIgnoreCase)))
                {
                    Columns.Insert(0, new MixDbColumnViewModel()
                    {
                        DisplayName = "Created By",
                        SystemName = fieldNameSrv.CreatedBy,
                        DataType = MixDataType.String,
                        ColumnConfigurations = new Shared.Models.ColumnConfigurations()
                        {
                            IsUnique = true,
                            IsRequire = true
                        }
                    });
                }
            }
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDbTable parentEntity, CancellationToken cancellationToken = default)
        {
            var fieldNameService = GetFielNameService();


            if (Columns != null)
            {
                if ((Type == MixDbTableType.AdditionalData || Type == MixDbTableType.GuidAdditionalData))
                {
                    if (!Columns.Any(m => m.SystemName == fieldNameService.ParentId))
                    {

                        Columns.Add(new()
                        {
                            DisplayName = "Parent Id",
                            SystemName = fieldNameService.ParentId,
                            DataType = Type == MixDbTableType.AdditionalData ? MixDataType.Reference : MixDataType.Guid
                        });
                    }
                    if (!Columns.Any(m => m.SystemName == fieldNameService.ParentType))
                    {
                        Columns.Add(new()
                        {
                            DisplayName = "Parent Type",
                            SystemName = fieldNameService.ParentType,
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
                    item.MixdbTableId = parentEntity.Id;
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
                    item.SourceDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync(cancellationToken);

                    await CreateRefColumn(item, fieldNameService, cancellationToken);

                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        private FieldNameService GetFielNameService()
        {
            if (MixDbDatabaseId.HasValue)
            {
                var dbContext = Context.MixDbDatabase.First(m => m.Id == MixDbDatabaseId);
                return new FieldNameService(dbContext.NamingConvention);
            }
            return new FieldNameService(MixDatabaseNamingConvention.TitleCase);
        }

        private async Task CreateRefColumn(MixDbTableRelationshipViewModel item, FieldNameService fieldNameService, CancellationToken cancellationToken = default)
        {
            if (item.Type == MixDbTableRelationshipType.OneToMany)
            {
                var referenceColumnName = fieldNameService.GetParentId(item.SourceDatabaseName);

                if (!Context.MixDbColumn.Any(m => m.MixDbTableName == item.DestinateDatabaseName && m.SystemName == referenceColumnName))
                {
                    var srcDb = Context.MixDbTable.FirstOrDefault(m => m.SystemName == item.SourceDatabaseName);
                    var destDb = Context.MixDbTable.FirstOrDefault(m => m.SystemName == item.DestinateDatabaseName);
                    var refCol = new MixDbColumnViewModel(UowInfo)
                    {
                        MixDbTableName = item.DestinateDatabaseName,
                        MixdbTableId = destDb.Id,
                        DataType = srcDb.Type == MixDbTableType.GuidService ? MixDataType.Guid : MixDataType.Integer,
                        CreatedBy = CreatedBy,
                        DisplayName = item.ReferenceColumnName.ToTitleCase(),
                        SystemName = referenceColumnName
                    };

                    await refCol.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(refCol.ModifiedEntities);
                }
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            // Exception: This MySqlConnection is already in use. See https://fl.vu/mysql-conn-reuse when delete nested entity using Repository
            //await MixDataContentValueViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
            //await MixDataViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
            //await MixDbColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDbDatabaseId == Id);
            foreach (var col in Columns)
            {
                col.SetUowInfo(UowInfo, CacheService);
                await col.DeleteAsync(cancellationToken);
            }

            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion

        public override void Duplicate()
        {
            Id = default;
            DisplayName = $"Duplicated {DisplayName}";
            SystemName = $"duplicated{SystemName}";

            Columns.ForEach(p => p.Id = default);
            Relationships.ForEach(p => p.Id = default);
        }
    }
}
