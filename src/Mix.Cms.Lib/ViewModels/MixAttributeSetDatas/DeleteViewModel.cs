using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, DeleteViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        #endregion Models
        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override RepositoryResponse<bool> RemoveRelatedModels(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var removeFields = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = removeFields.IsSucceed,
                Data = removeFields.IsSucceed
            };
        }
        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var removeFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = removeFields.IsSucceed,
                Data = removeFields.IsSucceed
            };
        }
        #endregion

    }
}
