using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Mix.Cms.Lib.Enums;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixDatabaseContentAssociation, UpdateViewModel>
    {
        public UpdateViewModel(MixDatabaseContentAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }
        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseContentAssociationType ParentType { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

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
        public MixAttributeSetDatas.UpdateViewModel Data { get; set; }

        #endregion Views

        #region overrides

        public override MixDatabaseContentAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
                Status = Status == default ? Enum.Parse<MixContentStatus>(MixService.GetConfig<string>(AppSettingKeywords.DefaultContentStatus)) : Status;
            }

            var getData = MixAttributeSetDatas.UpdateViewModel.Repository.GetSingleModel(p => p.Id == DataId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                Data = getData.Data;
            }
            AttributeSetName = _context.MixAttributeSet.FirstOrDefault(m => m.Id == AttributeSetId)?.Name;
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    tasks.Add(Task.Factory.StartNew(() =>
        //    {
        //        Data.GenerateCache(Data.Model, Data);
        //    }));
        //    return tasks;
        //}

        #endregion overrides
    }
}