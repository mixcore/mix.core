using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ImportViewModel
        : ViewModelBase<MixCmsContext, MixRelatedAttributeData, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public int ParentType { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isProcessed")]
        public bool IsProcessed { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //var getPost = MixAttributeSetDatas.ReadViewModel.Repository.GetSingleModel(
            //    m => m.Id == Id && m.Specificulture == Specificulture
            //    , _context: _context, _transaction: _transaction);
            //if (getPost.IsSucceed)
            //{
            //    this.RelatedAttributeData = getPost.Data;
            //}
        }

        #endregion Overrides
    }
}