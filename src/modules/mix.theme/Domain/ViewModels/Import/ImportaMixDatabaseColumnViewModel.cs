using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Enums;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Lib.Entities.Cms;
using Mix.Theme.Domain.Models;
using Mix.Lib.ViewModels.Cms;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportaMixDatabaseColumnViewModel
      : MixDatabaseColumnViewModelBase<ImportaMixDatabaseColumnViewModel>
    {
        #region Properties

        #region Models
        public string Specificulture { get; set; }
        public JArray JOptions { get; set; }
        public bool IsRegex { get { return !string.IsNullOrEmpty(Regex); } }
        #endregion Models

        #region Views

        public ColumnConfigurations ColumnConfigurations { get; set; } = new ColumnConfigurations();

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportaMixDatabaseColumnViewModel() : base()
        {
        }

        public ImportaMixDatabaseColumnViewModel(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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