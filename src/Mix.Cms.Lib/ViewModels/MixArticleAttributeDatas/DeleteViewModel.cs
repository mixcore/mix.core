using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeDatas
{
    public class DeleteViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeData, DeleteViewModel>
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
        public DeleteViewModel(MixPostAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public DeleteViewModel() : base()
        {
        }
        #region Overrides

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await MixArticleAttributeValues.DeleteViewModel.ModelRepository.RemoveListModelAsync(v => v.DataId == Id && v.Specificulture == Specificulture,
                _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Exception = result.Exception,
                Errors = result.Errors
            };
        }
        public override RepositoryResponse<bool> RemoveRelatedModels(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = MixArticleAttributeValues.DeleteViewModel.ModelRepository.RemoveListModel(v => v.DataId == Id && v.Specificulture == Specificulture,
                _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Exception = result.Exception,
                Errors = result.Errors
            };
        }

        #endregion
    }
}
