using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeSets
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixArticleAttributeSet, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixArticleAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views
        [JsonProperty("attributeSet")]
        public MixAttributeSets.ReadMvcArticleViewModel MixAttributeSet { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixAttributeSet = MixAttributeSets.ReadMvcArticleViewModel.Repository.GetSingleModel(
                p => p.Id == AttributeSetId
            , _context: _context, _transaction: _transaction
            ).Data;

            // Load all article Data
            MixAttributeSet.LoadArticleData(ArticleId, Specificulture, _context: _context, _transaction: _transaction);
        }

        #endregion overrides
    }
}
