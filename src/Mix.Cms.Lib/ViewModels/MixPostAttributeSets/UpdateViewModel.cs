using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeSets
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeSet, UpdateViewModel>
    {
        public UpdateViewModel(MixPostAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #region Views

        [JsonProperty("attributeSet")]
        public MixAttributeSets.ContentUpdateViewModel MixAttributeSet { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getAttributeSet = MixAttributeSets.ContentUpdateViewModel.Repository.GetSingleModel(p => p.Id == AttributeSetId, _context: _context, _transaction: _transaction
            );
            if (getAttributeSet.IsSucceed)
            {
                MixAttributeSet = getAttributeSet.Data;
            }
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPostAttributeSet parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            //Save current set attribute
            var saveResult = await MixAttributeSet.SaveModelAsync(true, _context, _transaction);
            ViewModelHelper.HandleResult(saveResult, ref result);
            return result;
        }

        #endregion Async

        #endregion overrides
    }
}