using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ODataDeleteViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeSetData, ODataDeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public ODataDeleteViewModel() : base()
        {
        }

        public ODataDeleteViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override RepositoryResponse<bool> RemoveRelatedModels(ODataDeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeValues = MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModel(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeValues, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = RemoveRelated(_context, _transaction);
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

        public override async System.Threading.Tasks.Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(ODataDeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Remove values
            var removeFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.DataId == Id && f.Specificulture == Specificulture, _context, _transaction);
            ViewModelHelper.HandleResult(removeFields, ref result);

            // remove related navs
            if (result.IsSucceed)
            {
                var removeRelated = await RemoveRelatedAsync(_context, _transaction);
                ViewModelHelper.HandleResult(removeRelated, ref result);
            }

            if (result.IsSucceed)
            {
                var removeChildFields = await MixAttributeSetValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => (f.DataId == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChildFields, ref result);
                var removeChilds = await MixAttributeSetDatas.DeleteViewModel.Repository.RemoveListModelAsync(false, f => (f.Id == Id) && f.Specificulture == Specificulture, _context, _transaction);
                ViewModelHelper.HandleResult(removeChilds, ref result);
            }
            return result;
        }

        private RepositoryResponse<bool> RemoveRelated(MixCmsContext context, IDbContextTransaction transaction)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Get All Navs
            var navs = context.MixRelatedAttributeData.Where(d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
            // Filter child navs
            foreach (var item in navs)
            {
                // Count parent navs
                var count = context.MixRelatedAttributeData.Count(m => m.Id == item.Id && m.Specificulture == item.Specificulture);
                // Remove current Navs
                var removeNav = MixRelatedAttributeDatas.ODataDeleteViewModel.Repository.RemoveModel(item, context, transaction);
                ViewModelHelper.HandleResult(removeNav, ref result);

                // If only 1 nav => remove child data
                if (result.IsSucceed && count == 1)
                {
                    var removeData = MixAttributeSetDatas.ODataDeleteViewModel.Repository.RemoveModel(
                        m => m.Id == item.Id && m.Specificulture == item.Specificulture, context, transaction);
                    ViewModelHelper.HandleResult(removeData, ref result);
                }
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> RemoveRelatedAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Get All Navs
            var navs = context.MixRelatedAttributeData.Where(d => (d.Id == Id || d.ParentId == Id) && d.Specificulture == Specificulture);
            // Filter child navs
            foreach (var item in navs)
            {
                // Count parent navs
                var count = context.MixRelatedAttributeData.Count(m => m.Id == item.Id && m.Specificulture == item.Specificulture);
                // Remove current Navs
                var removeNav = await MixRelatedAttributeDatas.ODataDeleteViewModel.Repository.RemoveModelAsync(item, context, transaction);
                ViewModelHelper.HandleResult(removeNav, ref result);

                // If only 1 nav => remove child data
                if (result.IsSucceed && count == 1)
                {
                    var removeData = await MixAttributeSetDatas.ODataDeleteViewModel.Repository.RemoveModelAsync(
                        m => m.Id == item.Id && m.Specificulture == item.Specificulture, context, transaction);
                    ViewModelHelper.HandleResult(removeData, ref result);
                }
                if (!result.IsSucceed)
                {
                    break;
                }
            }
            return result;
        }

        #endregion Overrides
    }
}