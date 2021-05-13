using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixPortalPagePortalPages
{
    public class ReadPermissionViewModel
       : ViewModelBase<MixCmsContext, MixPortalPageNavigation, ReadPermissionViewModel>
    {
        public ReadPermissionViewModel(MixPortalPageNavigation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadPermissionViewModel() : base()
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
        public MixPortalPages.ReadRolePermissionViewModel Page { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPage = MixPortalPages.ReadRolePermissionViewModel.Repository.GetSingleModel(p => p.Id == PageId
            , _context, _transaction
            );
            if (getPage.IsSucceed)
            {
                Page = getPage.Data;
            }
        }

        #endregion overrides

        #region Expands

        public static async System.Threading.Tasks.Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfosAsync(List<MixPortalPagePortalPages.ReadViewModel> cates)
        {
            MixCmsContext context = new MixCmsContext();
            var transaction = context.Database.BeginTransaction();
            var result = new RepositoryResponse<List<ReadViewModel>>();
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
                UnitOfWorkHelper<MixCmsContext>.HandleException<ReadViewModel>(ex, true, transaction);
                return new RepositoryResponse<List<ReadViewModel>>()
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