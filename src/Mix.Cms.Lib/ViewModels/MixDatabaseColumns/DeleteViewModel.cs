using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseColumns
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseColumn, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attributesetId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("referenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("dataType")]
        public int DataType { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isRequire")]
        public bool IsRequire { get; set; }

        [JsonProperty("isEncrypt")]
        public bool IsEncrypt { get; set; }

        [JsonProperty("isSelect")]
        public bool IsSelect { get; set; }

        [JsonProperty("isUnique")]
        public bool IsUnique { get; set; }

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

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region overrides

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(DeleteViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var removeDataIds = _context.MixDatabaseDataValue.Where(m => m.MixDatabaseColumnId == Id).Select(m => m.DataId).ToList();
            var removeFieldValues = await MixDatabaseDataValues.DeleteViewModel.Repository.RemoveListModelAsync(false, f => f.MixDatabaseColumnId == Id
            , _context, _transaction);
            ViewModelHelper.HandleResult(removeFieldValues, ref result);
            if (result.IsSucceed)
            {
                foreach (var item in removeDataIds)
                {
                    _ = MixCacheService.RemoveCacheAsync(typeof(MixDatabaseData), item);
                }
            }
            return result;
        }
        public override async Task RemoveCache(MixDatabaseColumn model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string key = $"_{MixDatabaseId}";
            await MixCacheService.RemoveCacheAsync(typeof(MixDatabase), key);
            await base.RemoveCache(model, _context, _transaction);
        }
        #endregion overrides
    }
}