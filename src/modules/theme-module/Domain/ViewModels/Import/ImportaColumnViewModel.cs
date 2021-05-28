using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Enums;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Lib.Entities.Cms;
using Mix.Theme.Domain.Models;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportaColumnViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseColumn, ImportaColumnViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public int MixDatabaseId { get; set; }

        public string MixDatabaseName { get; set; }

        public string Configurations { get; set; }

        public int? ReferenceId { get; set; }

        public string Regex { get; set; }

        public bool IsRegex { get { return !string.IsNullOrEmpty(Regex); } }

        public string Title { get; set; }

        public MixDataType DataType { get; set; }

        public string DefaultValue { get; set; }

        public string Options { get; set; } = "[]";

        public JArray JOptions { get; set; }

        public string Name { get; set; }

        public bool IsRequire { get; set; }

        public bool IsEncrypt { get; set; }

        public bool IsSelect { get; set; }

        public bool IsUnique { get; set; }

        public bool IsMultiple { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        public ColumnConfigurations ColumnConfigurations { get; set; } = new ColumnConfigurations();

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportaColumnViewModel() : base()
        {
        }

        public ImportaColumnViewModel(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (MixDatabaseName != "sys_additional_field")
                {
                    // Check if there is field name in the same attribute set
                    IsValid = !_context.MixDatabaseColumn.Any(
                        f => f.Id != Id && f.Name == Name && f.MixDatabaseId == MixDatabaseId);
                    if (!IsValid)
                    {
                        Errors.Add($"Field {Name} Existed");
                    }
                }
            }
        }

        public override MixDatabaseColumn ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            Options = JOptions?.ToString();
            Configurations = JObject.FromObject(ColumnConfigurations).ToString(Formatting.None);
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(Options))
            {
                JOptions = JArray.Parse(Options);
            }

            ColumnConfigurations = string.IsNullOrEmpty(Configurations) ? new ColumnConfigurations()
                    : JObject.Parse(Configurations).ToObject<ColumnConfigurations>();
        }

        public override Task RemoveCache(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            using (_context ??= new MixCmsContext())
            {
                var relatedDatabaseId = _context.MixDatabase.Where(m => m.Id == MixDatabaseId).Select(m => m.Id);
                MixCacheService.RemoveCacheAsync(typeof(MixDatabase), relatedDatabaseId.ToString());
                return base.RemoveCache(model, _context, _transaction);
            }
        }
        #endregion Overrides
    }
}