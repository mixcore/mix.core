using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeValues
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixPostAttributeValue, ReadMvcViewModel>
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

        [JsonProperty("decryptValue")]
        public string DecryptValue {
            get {
                return !string.IsNullOrEmpty(EncryptKey) ?
                        AesEncryptionHelper.DecryptStringFromBytes_Aes(EncryptValue, EncryptKey).Data?.Data
                    : null;
            }
        }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixPostAttributeValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            switch (DataType)
            {
                // Convert  markdown value => html
                case 21:
                    StringValue = CommonMark.CommonMarkConverter.Convert(StringValue);
                    break;
            }
        }

        #endregion Overrides
    }
}