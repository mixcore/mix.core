using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeValues
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixArticleAttributeValue, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("dataId")]
        public string DataId { get; set; }
        [JsonProperty("attributeName")]
        public string AttributeName { get; set; }
        [JsonProperty("ArticleId")]
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
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("customClass")]
        public string CustomClass { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        #endregion Models

       
        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixArticleAttributeValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}
