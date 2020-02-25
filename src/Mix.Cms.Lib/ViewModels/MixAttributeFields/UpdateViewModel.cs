using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Cms.Lib.ViewModels.MixAttributeFields
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeField, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("referenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("dataType")]
        public MixEnums.MixDataType DataType { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonIgnore]
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

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeField model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                // Check if there is field name in the same attribute set
                IsValid = !Repository.CheckIsExists(f => f.Name == Name && f.AttributeSetId == AttributeSetId);
                if (!IsValid)
                {
                    Errors.Add($"Field {Name} Existed");
                }
            }
        }

        public override MixAttributeField ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            Options = JOptions?.ToString();
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(Options))
            {
                JOptions = JArray.Parse(Options);
            }
        }

        #endregion Overrides
    }
}