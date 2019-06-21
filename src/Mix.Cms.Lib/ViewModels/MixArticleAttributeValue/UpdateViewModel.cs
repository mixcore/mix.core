using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeValues
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixArticleAttributeValue, UpdateViewModel>
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
        public int DataType { get; set; }
        [JsonProperty("attributeName")]
        public string AttributeName { get; set; }
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
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
        #endregion

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixArticleAttributeValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Field = MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId, _context, _transaction).Data;
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
        public override MixArticleAttributeValue ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }

            if (Field.IsEncrypt)
            {

            }
            return base.ParseModel(_context, _transaction);
        }
        #endregion
    }
}
