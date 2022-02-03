using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDataAssociations
{
    public class FormViewModel
       : ViewModelBase<MixCmsContext, MixDatabaseDataAssociation, FormViewModel>
    {
        public FormViewModel(MixDatabaseDataAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public FormViewModel() : base()
        {
        }

        #region Model

        /*
         * Attribute Set Data Id
         */

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("dataId")]
        public string DataId { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseParentType ParentType { get; set; }

        [JsonProperty("mixDatabaseId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

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

        #endregion Model

        #region Views
        [JsonProperty("isActived")]
        public bool IsActived { get; set; }
        [JsonProperty("attributeData")]
        public MixDatabaseDatas.FormViewModel AttributeData { get; set; }

        #endregion Views

        #region overrides

        public override MixDatabaseDataAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
                Status = Status == default ? Enum.Parse<MixContentStatus>(MixService.GetAppSetting<string>(
                    MixAppSettingKeywords.DefaultContentStatus)) : Status;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixDatabaseDatas.FormViewModel.Repository.GetSingleModel(p => p.Id == DataId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                AttributeData = getData.Data;
            }
            MixDatabaseName = _context.MixDatabase.FirstOrDefault(m => m.Id == MixDatabaseId)?.Name;
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (string.IsNullOrEmpty(ParentId) || ParentId == "0")
                {
                    IsValid = false;
                    Errors.Add("Invalid Parent Id");
                }
                if (MixDatabaseId <= 0 || string.IsNullOrEmpty(MixDatabaseName))
                {
                    IsValid = false;
                    Errors.Add("Invalid Mix Database");
                }
            }
        }

        public override async Task<RepositoryResponse<FormViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<FormViewModel>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                if (AttributeData != null && string.IsNullOrEmpty(AttributeData.Id))
                {
                    var saveData = await AttributeData.SaveModelAsync(true, context, transaction);
                    if (!saveData.IsSucceed)
                    {
                        result.IsSucceed = false;
                        result.Errors = saveData.Errors;
                        result.Exception = saveData.Exception;
                    }
                    else
                    {
                        DataId = saveData.Data.Id;
                    }
                }
                if (result.IsSucceed)
                {
                    result = await base.SaveModelAsync(true, context, transaction);
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<FormViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    context.Dispose();
                }
            }
        }

        public override async Task<RepositoryResponse<bool>> CloneSubModelsAsync(MixDatabaseDataAssociation parent, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (AttributeData != null)
            {
                AttributeData.Cultures = Cultures;
                var model = AttributeData.ParseModel(_context, _transaction);
                var cloneValue = await AttributeData.CloneAsync(model, Cultures, _context, _transaction);
                ViewModelHelper.HandleResult(cloneValue, ref result);
            }
            return result;
        }
        #endregion overrides
    }
}