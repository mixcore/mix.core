using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;

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
        #endregion

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
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
            Priority = Field.Priority;
            DataType = Field.DataType;

            return base.ParseModel(_context, _transaction);
        }
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Field = MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId).Data;
        }
        #endregion

        #region Expands

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
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
            }
        }

        #endregion
    }
}
