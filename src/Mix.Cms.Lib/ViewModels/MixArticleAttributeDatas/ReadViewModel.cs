using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeDatas
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixArticleAttributeData, ReadViewModel>
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
        #endregion
        public ReadViewModel(MixArticleAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

    }
}
