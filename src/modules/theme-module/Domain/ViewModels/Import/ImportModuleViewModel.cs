using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.ViewModels.Cms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportModuleViewModel : MixModuleViewModelBase<ImportModuleViewModel>
    {
        #region Properties
        public List<MixModuleDataViewModel> Data { get; set; } = new List<MixModuleDataViewModel>();

        public List<MixModulePostViewModel> PostNavs { get; set; } // Parent to Posts

        //Parent Post Id
        public string PostId { get; set; }

        //Parent Category Id
        public int PageId { get; set; }

        public bool IsExportData { get; set; }

        public ImportMixDataAssociationViewModel RelatedData { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        #endregion

        #region Overrides

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid && Id == 0)
            {
                IsValid = !Repository.CheckIsExists(m => m.Name == Name && m.Specificulture == Specificulture
                , _context, _transaction);
                if (!IsValid)
                {
                    Errors.Add("Module Name Existed");
                }
            }
        }

        public override MixModule ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                LastModified = DateTime.UtcNow;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            GetAdditionalData(Id.ToString(), MixDatabaseParentType.Module, _context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(
            MixModule parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            foreach (var item in Data)
            {
                if (result.IsSucceed)
                {
                    item.Specificulture = parent.Specificulture;
                    item.ModuleId = parent.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
            }
            return result;
        }

        #region Async

        public override Task<RepositoryResponse<MixModule>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
        }

        #endregion Async

        #endregion Overrides

        #region Expand

        public void LoadData(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<PaginationModel<MixModuleDataViewModel>> getDataResult = MixModuleDataViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       , "Priority", 0, null, null
                       , _context, _transaction);
        }

        public List<MixModulePostViewModel> GetPostNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixModulePostViewModel.Repository.GetModelListBy(
                m => m.Specificulture == Specificulture && m.ModuleId == Id,
                context, transaction).Data;
        }

        private void GetAdditionalData(string id, MixDatabaseParentType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getRelatedData = ImportMixDataAssociationViewModel.Repository.GetFirstModel(
                        m => m.Specificulture == Specificulture && m.ParentType == type
                            && m.ParentId == id, context, transaction);
            if (getRelatedData.IsSucceed)
            {
                RelatedData = (getRelatedData.Data);
            }
        }

        #endregion Expand
    }
}
