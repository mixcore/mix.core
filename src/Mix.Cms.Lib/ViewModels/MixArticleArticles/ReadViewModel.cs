using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixArticleArticles
{
    public class ReadViewModel
        : ViewModelBase<MixCmsContext, MixRelatedArticle, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("sourceId")]
        public int SourceId { get; set; }

        [JsonProperty("destinationId")]
        public int DestinationId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("relatedArticle")]
        public MixArticles.ReadListItemViewModel RelatedArticle { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixRelatedArticle model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getArticle = MixArticles.ReadListItemViewModel.Repository.GetSingleModel(
                m => m.Id == DestinationId && m.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction);
            if (getArticle.IsSucceed)
            {
                this.RelatedArticle = getArticle.Data;
            }
        }

        public override MixRelatedArticle ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}
