using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System.Linq;

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
            var removeFields = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeFields, ref result);

            // remove related values
            if (result.IsSucceed)
            {
                var subData = _context.MixRelatedAttributeData.Where(d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
                foreach (var item in subData)
                {
                    var removeChildFields = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => (f.DataId == item.Id || f.DataId == item.ParentId) && f.Specificulture == Specificulture, _context, _transaction);
                    ViewModelHelper.HandleResult(removeChildFields, ref result);
                    var removeChilds = MixAttributeSetDatas.DeleteViewModel.Repository.RemoveListModel(false, f => (f.Id == item.Id || f.Id == item.ParentId) && f.Specificulture == Specificulture, _context, _transaction);
                    ViewModelHelper.HandleResult(removeChilds, ref result);
                }
            }
            return result;
        }
        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeFields, ref result);

            // remove subdata values
            if (result.IsSucceed)
            {
                var subData = _context.MixRelatedAttributeData.Where(d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
                foreach (var item in subData)
                {
                    var removeChildFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => (f.DataId == item.Id || f.DataId == item.ParentId) && f.Specificulture == Specificulture, _context, _transaction);
                    ViewModelHelper.HandleResult(removeChildFields, ref result);
                    var removeChilds = await MixAttributeSetDatas.DeleteViewModel.Repository.RemoveListModelAsync(false, f => (f.Id == item.Id || f.Id == item.ParentId) && f.Specificulture == Specificulture, _context, _transaction);
                    ViewModelHelper.HandleResult(removeChilds, ref result);
                }
            }
            return result;
        }
        #endregion

    }
}
