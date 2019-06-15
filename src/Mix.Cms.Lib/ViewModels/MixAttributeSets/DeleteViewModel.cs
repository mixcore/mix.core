using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, DeleteViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        #endregion Models
        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override RepositoryResponse<bool> RemoveRelatedModels(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var removeFields = MixAttributeFields.DeleteViewModel.Repository.RemoveListModel(false, f => f.AttributeSetId == Id, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = removeFields.IsSucceed,
                Data = removeFields.IsSucceed
            };
        }
        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var removeFields = await MixAttributeFields.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.AttributeSetId == Id, _context, _transaction);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = removeFields.IsSucceed,
                Data = removeFields.IsSucceed
            };
        }
        #endregion

    }
}
