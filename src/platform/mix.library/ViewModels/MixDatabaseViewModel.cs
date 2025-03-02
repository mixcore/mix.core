using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.ViewModels.ReadOnly;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseViewModel : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, MixDatabaseViewModel>
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
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseViewModel()
        {
        }

        public MixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task Validate(CancellationToken cancellationToken)
        {
            if (await Context.MixDatabase.AnyAsync(p => p.Id != Id && p.SystemName == SystemName, cancellationToken))
            {
                IsValid = false;
                Errors.Add(new ValidationResult("Database name must be unique."));
            }

            await base.Validate(cancellationToken);
        }

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var columnRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo, CacheService);
            Columns = await columnRepo.GetListAsync(c => c.MixDatabaseId == Id, cancellationToken);
            Relationships = await MixDatabaseRelationshipViewModel.GetRepository(UowInfo, CacheService).GetListAsync(c => c.ParentId == Id, cancellationToken);
            if (MixDatabaseContextId.HasValue)
            {
                MixDatabaseContext = await MixDatabaseContextReadViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m => m.Id == MixDatabaseContextId.Value);
                NamingConvention = MixDatabaseContext.NamingConvention;
            }
        }

        public override Task<MixDatabase> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (Id == default)
            {
                var fieldNameSrv = new FieldNameService(NamingConvention);
                if (!Columns.Any(m => string.Equals(m.SystemName, fieldNameSrv.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    Columns.Insert(0, new MixDatabaseColumnViewModel()
                    {
                        DisplayName = "Id",
                        SystemName = fieldNameSrv.Id,
                        DataType = Type == MixDatabaseType.GuidService ? MixDataType.Guid : MixDataType.Integer,
                        ColumnConfigurations = new Shared.Models.ColumnConfigurations()
                        {
                            IsUnique = true,
                            IsRequire = true
                        }
                    });
                }
                if (!Columns.Any(m => string.Equals(m.SystemName, fieldNameSrv.CreatedBy, StringComparison.OrdinalIgnoreCase)))
                {
                    Columns.Insert(0, new MixDatabaseColumnViewModel()
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

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity, CancellationToken cancellationToken = default)
        {
            var fieldNameService = GetFielNameService();


            if (Columns != null)
            {
                if ((Type == MixDatabaseType.AdditionalData || Type == MixDatabaseType.GuidAdditionalData))
                {
                    if (!Columns.Any(m => m.SystemName == fieldNameService.ParentId))
                    {

                        Columns.Add(new()
                        {
                            DisplayName = "Parent Id",
                            SystemName = fieldNameService.ParentId,
                            DataType = Type == MixDatabaseType.AdditionalData ? MixDataType.Reference : MixDataType.Guid
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

                    await CreateRefColumn(item, fieldNameService, cancellationToken);

                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        private FieldNameService GetFielNameService()
        {
            if (MixDatabaseContextId.HasValue)
            {
                var dbContext = Context.MixDatabaseContext.First(m => m.Id == MixDatabaseContextId);
                return new FieldNameService(dbContext.NamingConvention);
            }
            return new FieldNameService(MixDatabaseNamingConvention.TitleCase);
        }

        private async Task CreateRefColumn(MixDatabaseRelationshipViewModel item, FieldNameService fieldNameService, CancellationToken cancellationToken = default)
        {
            if (item.Type == MixDatabaseRelationshipType.OneToMany)
            {
                var referenceColumnName = fieldNameService.GetParentId(item.SourceDatabaseName);

                if (!Context.MixDatabaseColumn.Any(m => m.MixDatabaseName == item.DestinateDatabaseName && m.SystemName == referenceColumnName))
                {
                    var srcDb = Context.MixDatabase.FirstOrDefault(m => m.SystemName == item.SourceDatabaseName);
                    var destDb = Context.MixDatabase.FirstOrDefault(m => m.SystemName == item.DestinateDatabaseName);
                    var refCol = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseName = item.DestinateDatabaseName,
                        MixDatabaseId = destDb.Id,
                        DataType = srcDb.Type == MixDatabaseType.GuidService ? MixDataType.Guid : MixDataType.Integer,
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
