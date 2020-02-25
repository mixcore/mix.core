using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeSets
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeSet, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixPostAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        [JsonProperty("attributeSet")]
        public MixAttributeSets.ReadMvcPostViewModel MixAttributeSet { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            MixAttributeSet = MixAttributeSets.ReadMvcPostViewModel.Repository.GetSingleModel(
                p => p.Id == AttributeSetId
            , _context: _context, _transaction: _transaction
            ).Data;

            // Load all post Data
            MixAttributeSet.LoadPostData(PostId, Specificulture, _context: _context, _transaction: _transaction);
        }

        #endregion overrides
    }
}