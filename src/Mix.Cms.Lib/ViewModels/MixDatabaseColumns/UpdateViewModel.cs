using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseColumns
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseColumn, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("mixDatabaseId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("configurations")]
        public string Configurations { get; set; }

        [JsonProperty("referenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("isRegex")]
        public bool IsRegex { get { return !string.IsNullOrEmpty(Regex); } }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; } = MixDataType.Text;

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("strOptions")]
        public string Options { get; set; } = "[]";

        [JsonProperty("options")]
        public JArray JOptions { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isRequire")]
        public bool IsRequire { get; set; }

        [JsonProperty("isEncrypt")]
        public bool IsEncrypt { get; set; }

        [JsonProperty("isSelect")]
        public bool IsSelect { get; set; }

        [JsonProperty("isUnique")]
        public bool IsUnique { get; set; }

        [JsonProperty("isMultiple")]
        public bool IsMultiple { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("columnConfigurations")]
        public ColumnConfigurations ColumnConfigurations { get; set; } = new ColumnConfigurations();

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (MixDatabaseName != "sys_additional_column")
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
            _context ??= new MixCmsContext();
            var relatedDatabaseId = _context.MixDatabase.Where(m => m.Id == MixDatabaseId).Select(m => m.Id);
            MixCacheService.RemoveCacheAsync(typeof(MixDatabase), relatedDatabaseId.ToString());
            return base.RemoveCache(model, _context, _transaction);
        }
        #endregion Overrides
    }
}