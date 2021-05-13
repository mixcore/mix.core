using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDatas
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override RepositoryResponse<bool> RemoveRelatedModels(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeValues = MixDatabaseDataValues.DeleteViewModel.Repository.RemoveListModel(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeValues, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = MixDatabaseDataAssociations.DeleteViewModel.Repository.RemoveListModel(true, d => (d.DataId == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
                ViewModelHelper.HandleResult(removeRelated, ref result);
            }

            if (result.IsSucceed)
            {
                var removeChildFields = MixDatabaseDataValues.DeleteViewModel.Repository.RemoveListModel(false, f => (f.DataId == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChildFields, ref result);
                var removeChilds = MixDatabaseDatas.DeleteViewModel.Repository.RemoveListModel(false, f => (f.Id == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChilds, ref result);
            }
            return result;
        }

        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeFields = await MixDatabaseDataValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeFields, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = await MixDatabaseDataAssociations.DeleteViewModel.Repository.RemoveListModelAsync
                    (true, d => (d.DataId == Id || d.ParentId == Id) && d.Specificulture == Specificulture
                    , _context, _transaction);
                ViewModelHelper.HandleResult(removeRelated, ref result);
            }

            if (result.IsSucceed)
            {
                var removeChildFields = await MixDatabaseDataValues.DeleteViewModel.Repository.RemoveListModelAsync(
                    false, f => (f.DataId == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChildFields, ref result);
                var removeChilds = await MixDatabaseDatas.DeleteViewModel.Repository.RemoveListModelAsync(
                    false, f => (f.Id == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChilds, ref result);
            }
            return result;
        }

        #endregion Overrides
    }
}