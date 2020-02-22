using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeFieldId")]
        public int AttributeFieldId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("dataType")]
        public MixEnums.MixDataType DataType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("attributeFieldName")]
        public string AttributeFieldName { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("dataId")]
        public string DataId { get; set; }

        [JsonProperty("dateTimeValue")]
        public DateTime? DateTimeValue { get; set; }

        [JsonProperty("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonProperty("integerValue")]
        public int? IntegerValue { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("encryptValue")]
        public string EncryptValue { get; set; }

        [JsonProperty("encryptKey")]
        public string EncryptKey { get; set; }

        [JsonProperty("encryptType")]
        public int EncryptType { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("field")]
        public MixAttributeFields.UpdateViewModel Field { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
            //IsCache = false;
        }

        public UpdateViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            //IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override MixAttributeSetValue ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            Priority = Field?.Priority ?? Priority;
            DataType = Field?.DataType ?? DataType;

            AttributeFieldName = Field?.Name;
            AttributeFieldId = Field?.Id ?? 0;
            if (string.IsNullOrEmpty(StringValue) && !string.IsNullOrEmpty(Field?.DefaultValue))
            {
                ParseDefaultValue(Field.DefaultValue);
            }
            return base.ParseModel(_context, _transaction);
        }

        private void ParseDefaultValue(string defaultValue)
        {
            StringValue = defaultValue;
            switch (DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    break;

                case MixEnums.MixDataType.Date:
                    break;

                case MixEnums.MixDataType.Time:
                    break;

                case MixEnums.MixDataType.Double:
                    double.TryParse(defaultValue, out double doubleValue);
                    DoubleValue = DoubleValue;
                    break;

                case MixEnums.MixDataType.Boolean:
                    bool.TryParse(defaultValue, out bool boolValue);
                    BooleanValue = boolValue;
                    break;

                case MixEnums.MixDataType.Number:
                    int.TryParse(defaultValue, out int intValue);
                    IntegerValue = intValue;
                    break;
            }
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (AttributeFieldId > 0)
            {
                Field = MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId).Data;
                if (Field != null && DataType == MixEnums.MixDataType.Reference)
                {
                    AttributeSetName = _context.MixAttributeSet.FirstOrDefault(m => m.Id == Field.ReferenceId)?.Name;
                }
            }
            else // addictional field for page / post / module => id = 0
            {
                Field = new MixAttributeFields.UpdateViewModel()
                {
                    DataType = DataType,
                    Title = AttributeFieldName,
                    Name = AttributeFieldName,
                    Priority = Priority
                };
            }
        }

        #endregion Overrides

        #region Expands

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid && Field != null)
            {
                if (Field.IsUnique)
                {
                    var exist = _context.MixAttributeSetValue.Any(d => d.Specificulture == Specificulture
                        && d.StringValue == StringValue && d.Id != Id && d.DataId != DataId);
                    if (exist)
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} = {StringValue} is existed");
                    }
                }
                if (Field.IsRequire)
                {
                    if (string.IsNullOrEmpty(StringValue))
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} is required");
                    }
                }
                if (!string.IsNullOrEmpty(Field.Regex))
                {
                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(Field.Regex, RegexOptions.IgnoreCase);
                    Match m = r.Match(StringValue);
                    if (!m.Success)
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} is invalid");
                    }
                }
            }
        }

        #endregion Expands
    }
}