using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeValues
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixPostAttributeValue, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("dataId")]
        public string DataId { get; set; }

        [JsonProperty("attributeFieldId")]
        public int AttributeFieldId { get; set; }

        [JsonProperty("dataType")]
        public MixEnums.MixDataType DataType { get; set; }

        [JsonProperty("attributeName")]
        public string AttributeName { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonProperty("integerValue")]
        public int? IntegerValue { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("datetimeValue")]
        public DateTime? DateTimeValue { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("encryptValue")]
        public string EncryptValue { get; set; }

        [JsonProperty("encryptKey")]
        public string EncryptKey { get; set; }

        [JsonProperty("encryptType")]
        public int EncryptType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("field")]
        public MixAttributeFields.UpdateViewModel Field { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixPostAttributeValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Field = MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId, _context, _transaction).Data;
            Priority = Field.Priority;
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            Field = Field ?? MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId, _context, _transaction).Data;
            if (IsValid)
            {
                if (Field.IsRequire)
                {
                    IsValid = IsValid && !string.IsNullOrEmpty(StringValue);
                    if (!IsValid)
                    {
                        Errors.Add($"{Field.Title} is required");
                    }
                }
                // validate unique
                if (Field.IsUnique)
                {
                    IsValid = IsValid && Repository.Count(
                        f => f.AttributeFieldId == AttributeFieldId && f.Id != Id
                        && f.StringValue == StringValue && f.Specificulture == Specificulture, _context, _transaction)
                        .Data == 0;
                    if (!IsValid)
                    {
                        Errors.Add($"{Field.Title} is existed");
                    }
                }
            }
        }

        public override MixPostAttributeValue ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }

            if (Field.IsEncrypt && !string.IsNullOrEmpty(StringValue))
            {
                if (string.IsNullOrEmpty(EncryptValue))
                {
                    EncryptKey = Guid.NewGuid().ToString("N");
                    EncryptValue = AesEncryptionHelper.EncryptString(StringValue, EncryptKey);
                }
                StringValue = string.Empty;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}