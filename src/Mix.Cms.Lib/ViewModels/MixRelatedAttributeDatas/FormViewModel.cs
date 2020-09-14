using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class FormViewModel
       : ViewModelBase<MixCmsContext, MixRelatedAttributeData, FormViewModel>
    {
        public FormViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }
        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixEnums.MixAttributeSetDataType ParentType { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Model

        #region Views

        [JsonProperty("attributeData")]
        public MixAttributeSetDatas.FormViewModel AttributeData { get; set; }

        #endregion Views

        #region overrides

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
                Status = Status == default ? Enum.Parse<MixEnums.MixContentStatus>(MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultContentStatus)) : Status;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSetDatas.FormViewModel.Repository.GetSingleModel(p => p.Id == DataId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                AttributeData = getData.Data;
            }
            AttributeSetName = _context.MixAttributeSet.FirstOrDefault(m => m.Id == AttributeSetId)?.Name;
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
                else
                {
                    DataId = AttributeData.Id;
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

        #endregion overrides
    }
}