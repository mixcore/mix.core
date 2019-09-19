using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ReadViewModel
        : ViewModelBase<MixCmsContext, MixRelatedAttributeData, ReadViewModel>
    {
        #region Properties

        #region Model
        [JsonProperty("id")]
        public string Id { get; set; }
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
        #endregion

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("RelatedAttributeData")]
        public MixAttributeSetDatas.ReadViewModel RelatedAttributeData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = MixAttributeSetDatas.ReadViewModel.Repository.GetSingleModel(
                m => m.Id == Id && m.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction);
            if (getPost.IsSucceed)
            {
                this.RelatedAttributeData = getPost.Data;
            }
        }

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}
