using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeSets
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixArticleAttributeSet, UpdateViewModel>
    {
        public UpdateViewModel(MixArticleAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }
        #region Views
        [JsonProperty("module")]
        public MixAttributeSets.ReadViewModel AttributeSet { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getAttributeSet = MixAttributeSets.ReadViewModel.Repository.GetSingleModel(p => p.Id == AttributeSetId, _context: _context, _transaction: _transaction
            );
            if (getAttributeSet.IsSucceed)
            {
                AttributeSet = getAttributeSet.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
