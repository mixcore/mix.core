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
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }
        
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
        public MixEnums.MixDataType DataType { get; set; }

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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("fieldConfigurations")]
        public FieldConfigurations FieldConfigurations { get; set; } = new FieldConfigurations();

        #endregion

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
                if (AttributeSetName != "sys_additional_field")
                {
                    // Check if there is field name in the same attribute set
                    IsValid = !Repository.CheckIsExists(
                        f => f.Id != Id && f.Name == Name && f.AttributeSetId == AttributeSetId);
                    if (!IsValid)
                    {
                        Errors.Add($"Field {Name} Existed");
                    }
                }
                else
                {
                    var currentField = Repository.GetSingleModel(m => m.Name == Name && m.DataType == DataType, _context, _transaction);
                    if (currentField.IsSucceed)
                    {
                        Id = currentField.Data.Id;
                        CreatedDateTime = currentField.Data.CreatedDateTime;
                    }
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
            Configurations = JObject.FromObject(FieldConfigurations).ToString(Formatting.None);
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(Options))
            {
                JOptions = JArray.Parse(Options);
            }
            
            FieldConfigurations = string.IsNullOrEmpty(Configurations) ? new FieldConfigurations()
                    : JObject.Parse(Configurations).ToObject<FieldConfigurations>();
        }

        #endregion Overrides
    }
}