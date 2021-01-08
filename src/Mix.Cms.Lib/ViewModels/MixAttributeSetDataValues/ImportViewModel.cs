﻿using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class ImportViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("attributeFieldId")]
        public int AttributeFieldId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("attributeFieldName")]
        public string AttributeFieldName { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

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

        [JsonProperty("field")]
        public Mix.Cms.Lib.ViewModels.MixAttributeFields.UpdateViewModel Field { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
            //IsCache = false;
        }

        public ImportViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
                case MixDataType.DateTime:
                    break;

                case MixDataType.Date:
                    break;

                case MixDataType.Time:
                    break;

                case MixDataType.Double:
                    double.TryParse(defaultValue, out double doubleValue);
                    DoubleValue = DoubleValue;
                    break;

                case MixDataType.Boolean:
                    bool.TryParse(defaultValue, out bool boolValue);
                    BooleanValue = boolValue;
                    break;

                case MixDataType.Integer:
                    int.TryParse(defaultValue, out int intValue);
                    IntegerValue = intValue;
                    break;
            }
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (AttributeFieldId > 0)
            {
                Field = Lib.ViewModels.MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId).Data;
                if (Field != null && DataType == MixDataType.Reference)
                {
                    AttributeSetName = _context.MixAttributeSet.FirstOrDefault(m => m.Id == Field.ReferenceId)?.Name;
                }
            }
            else // addictional field for page / post / module => id = 0
            {
                Field = new Lib.ViewModels.MixAttributeFields.UpdateViewModel()
                {
                    DataType = DataType,
                    Id = AttributeFieldId,
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