using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeDatas
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixArticleAttributeSet, UpdateViewModel>
    {
        #region Properties
        #region Models
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("attributeSetId")]
        public int SetAttributeId { get; set; }
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        #endregion

        #region Views
        [JsonProperty("data")]
        public List<MixArticleAttributeValues.UpdateViewModel> Data{ get; set; }
        #endregion

        #endregion
        public UpdateViewModel(MixArticleAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }
        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Data = MixArticleAttributeValues.UpdateViewModel.Repository.GetModelListBy(
                    v => v.DataId == Id && v.Specificulture == Specificulture, _context, _transaction).Data;
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
