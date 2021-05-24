using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPortalPagePortalPages
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPortalPageNavigation, UpdateViewModel>
    {
        public UpdateViewModel(MixPortalPageNavigation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        #region Properties

        #region Models

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("page")]
        public MixPortalPages.UpdateRolePermissionViewModel PortalPage { get; set; }

        [JsonProperty("parent")]
        public MixPortalPages.ReadViewModel ParentPage { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override MixPortalPageNavigation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPortalPageNavigation parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            if (PortalPage != null)
            {
                var result = await PortalPage.SaveModelAsync(false, _context, _transaction);
                return new RepositoryResponse<bool>()
                {
                    IsSucceed = result.IsSucceed,
                    Data = result.IsSucceed,
                    Errors = result.Errors,
                    Exception = result.Exception
                };
            }
            else
            {
                return await base.SaveSubModelsAsync(parent, _context, _transaction);
            }
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCategory = MixPortalPages.UpdateRolePermissionViewModel.Repository.GetSingleModel(
                p => p.Id == PageId, 
                _context, _transaction);
            if (getCategory.IsSucceed)
            {
                PortalPage = getCategory.Data;
            }
            //var getParent = MixPortalPages.ReadViewModel.Repository.GetSingleModel(p => p.Id == ParentId
            //    , _context: _context, _transaction: _transaction
            //);
            //if (getParent.IsSucceed)
            //{
            //    ParentPage = getParent.Data;
            //}
        }

        #endregion overrides

        #region Expands

        public static async System.Threading.Tasks.Task<RepositoryResponse<List<UpdateViewModel>>> UpdateInfosAsync(List<MixPortalPagePortalPages.UpdateViewModel> cates)
        {
            MixCmsContext context = new MixCmsContext();
            var transaction = context.Database.BeginTransaction();
            var result = new RepositoryResponse<List<UpdateViewModel>>();
            try
            {
                foreach (var item in cates)
                {
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Errors.AddRange(saveResult.Errors);
                        result.Exception = saveResult.Exception;
                        break;
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, true, transaction);
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<UpdateViewModel>(ex, true, transaction);
                return new RepositoryResponse<List<UpdateViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                //if current Context is Root
                transaction.Dispose();
                UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
            }
        }

        #endregion Expands
    }
}