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
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixDatabaseDataAssociation, UpdateViewModel>
    {
        public UpdateViewModel(MixDatabaseDataAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
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

        [JsonProperty("isClone")]
        public bool IsClone { get; set; }

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

        #endregion Model

        #region Views

        [JsonProperty("parentName")]
        public string ParentName { get; set; }

        [JsonProperty("data")]
        public MixDatabaseDatas.UpdateViewModel Data { get; set; }

        #endregion Views

        #region overrides

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                IsValid = ParentId != default;
                if (!IsValid)
                {
                    Errors.Add("Ivalid Parent");
                }
                IsValid = IsValid && !_context.MixDatabaseDataAssociation.Any(m =>
                            m.Id != Id
                            && m.Specificulture == Specificulture
                            && m.DataId == DataId
                            && m.ParentType == ParentType
                            && m.ParentId == ParentId);
                if (!IsValid)
                {
                    Errors.Add("This Association Existed");
                }
            }
        }

        public override MixDatabaseDataAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Status = Status == default ? Enum.Parse<MixContentStatus>(MixService.GetAppSetting<string>(
                    MixAppSettingKeywords.DefaultContentStatus)) : Status;
            }

            var getData = MixDatabaseDatas.UpdateViewModel.Repository.GetSingleModel(p => p.Id == DataId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                Data = getData.Data;
            }
            MixDatabaseName = _context.MixDatabase.FirstOrDefault(m => m.Id == MixDatabaseId)?.Name;
        }

        public override async Task<RepositoryResponse<bool>> CloneSubModelsAsync(MixDatabaseDataAssociation parent, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (Data != null)
            {
                Data.Cultures = Cultures;
                if (result.IsSucceed)
                {
                    var model = Data.ParseModel();
                    var cloneValue = await Data.CloneAsync(model, Cultures, _context, _transaction);
                    ViewModelHelper.HandleResult(cloneValue, ref result);
                }
            }
            return result;
        }

        #endregion overrides

        #region Expand

        public async Task<RepositoryResponse<UpdateViewModel>> DuplicateAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Id = null;
                var data = await Data.DuplicateAsync(context, transaction);
                DataId = data.Data.Id;
                return await SaveModelAsync(true, context, transaction);
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<UpdateViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    await transaction.CommitAsync();
                    await context.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}