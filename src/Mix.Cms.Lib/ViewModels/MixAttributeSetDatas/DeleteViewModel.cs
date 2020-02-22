using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

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
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeValues = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeValues, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = MixRelatedAttributeDatas.DeleteViewModel.Repository.RemoveListModel(true, d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
                ViewModelHelper.HandleResult(removeRelated, ref result);
            }

            if (result.IsSucceed)
            {
                var removeChildFields = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => (f.DataId == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChildFields, ref result);
                var removeChilds = MixAttributeSetDatas.DeleteViewModel.Repository.RemoveListModel(false, f => (f.Id == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChilds, ref result);
            }
            return result;
        }

        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeFields, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = await MixRelatedAttributeDatas.DeleteViewModel.Repository.RemoveListModelAsync
                    (true, d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture
                    , _context, _transaction);
                ViewModelHelper.HandleResult(removeRelated, ref result);
            }

            if (result.IsSucceed)
            {
                var removeChildFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(
                    false, f => (f.DataId == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChildFields, ref result);
                var removeChilds = await MixAttributeSetDatas.DeleteViewModel.Repository.RemoveListModelAsync(
                    false, f => (f.Id == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChilds, ref result);
            }
            return result;
        }

        #endregion Overrides
    }
}