using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, UpdateViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public int? Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("fields")]
        public string Fields { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("updatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        #endregion Models
        #region Views
        [JsonProperty("attributes")]
        public List<MixAttributeFields.UpdateViewModel> Attributes { get; set; }
        #endregion
        #endregion Properties
        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Attributes = MixAttributeFields.UpdateViewModel
                .Repository.GetModelListBy(a => a.AttributeSetId == Id).Data;
        }
        #endregion
    }
}
