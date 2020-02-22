using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixAttributeFields
{
    public class ODataMobileViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeField, ODataMobileViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attributesetId")]
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

        public ODataMobileViewModel() : base()
        {
        }

        public ODataMobileViewModel(MixAttributeField model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}