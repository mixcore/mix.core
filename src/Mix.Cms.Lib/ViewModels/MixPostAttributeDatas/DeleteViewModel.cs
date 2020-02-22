using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeDatas
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

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #endregion Properties

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
            var result = await MixPostAttributeValues.DeleteViewModel.ModelRepository.RemoveListModelAsync(v => v.DataId == Id && v.Specificulture == Specificulture,
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
            var result = MixPostAttributeValues.DeleteViewModel.ModelRepository.RemoveListModel(v => v.DataId == Id && v.Specificulture == Specificulture,
                _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result.IsSucceed,
                Exception = result.Exception,
                Errors = result.Errors
            };
        }

        #endregion Overrides
    }
}