using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixDatabases
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixDatabase, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ReferenceId")]
        public int? ReferenceId { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabase model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override RepositoryResponse<bool> RemoveRelatedModels(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var removeData = MixDatabaseDatas.DeleteViewModel.Repository.RemoveListModel(false, f => f.MixDatabaseId == Id, _context, _transaction);
            ViewModelHelper.HandleResult(removeData, ref result);
            if (result.IsSucceed)
            {
                var removeFields = MixDatabaseColumns.DeleteViewModel.Repository.RemoveListModel(false, f => f.MixDatabaseId == Id, _context, _transaction);
                ViewModelHelper.HandleResult(removeFields, ref result);
            }
            return result;
        }

        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var removeData = await MixDatabaseDatas.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.MixDatabaseId == Id, _context, _transaction);
            ViewModelHelper.HandleResult(removeData, ref result);
            if (result.IsSucceed)
            {
                var removeFields = await MixDatabaseColumns.DeleteViewModel.Repository.RemoveListModelAsync(
                    false, f => f.MixDatabaseId == Id || f.ReferenceId == Id, _context, _transaction);
                ViewModelHelper.HandleResult(removeFields, ref result);
            }
            return result;
        }

        #endregion Overrides
    }
}