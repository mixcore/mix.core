using Microsoft.EntityFrameworkCore.Storage;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Theme.Domain.Models;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Shared.Enums;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportaMixDatabaseColumnViewModel
      : ViewModelBase<MixCmsContextV2, MixDatabaseColumn, ImportaMixDatabaseColumnViewModel>
    {
        #region Properties
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; }
        public string Configurations { get; set; }
        public int? ReferenceId { get; set; }

        #region Views

        public ColumnConfigurations ColumnConfigurations { get; set; } = new ColumnConfigurations();

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportaMixDatabaseColumnViewModel() : base()
        {
        }

        public ImportaMixDatabaseColumnViewModel(
            MixDatabaseColumn model, MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null) 
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void Validate(MixCmsContextV2 _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (MixDatabaseName != "sys_additional_field")
                {
                    // Check if there is field name in the same attribute set
                    IsValid = !_context.MixDatabaseColumns.Any(
                        f => f.Id != Id && f.SystemName == SystemName && f.MixDatabaseId == MixDatabaseId);
                    if (!IsValid)
                    {
                        Errors.Add($"Field {SystemName} Existed");
                    }
                }
            }
        }

        public override MixDatabaseColumn ParseModel(MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            Configurations = JObject.FromObject(ColumnConfigurations).ToString(Formatting.None);
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            ColumnConfigurations = string.IsNullOrEmpty(Configurations) ? new ColumnConfigurations()
                    : JObject.Parse(Configurations).ToObject<ColumnConfigurations>();
        }

        public override Task RemoveCache(MixDatabaseColumn model, MixCmsContextV2 _context = null, IDbContextTransaction _transaction = null)
        {
            using (_context ??= new MixCmsContextV2())
            {
                var relatedDatabaseId = _context.MixDatabases.Where(m => m.Id == MixDatabaseId).Select(m => m.Id);
                MixCacheService.RemoveCacheAsync(typeof(MixDatabase), relatedDatabaseId.ToString());
                return base.RemoveCache(model, _context, _transaction);
            }
        }
        #endregion Overrides
    }
}