using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
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
        [JsonProperty("attributesetId")]
        public int AttributeSetId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("dataType")]
        public int DataType { get; set; }
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

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeField model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override MixAttributeField ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }
        #endregion
    }
}
