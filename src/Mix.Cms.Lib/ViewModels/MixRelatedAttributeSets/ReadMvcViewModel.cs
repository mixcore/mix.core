using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixRelatedAttributeSet, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixRelatedAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }
        #region Properties
        #region Models
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("attributeSetid")]
        public int AttributeSetId { get; set; }
        [JsonProperty("parentId")]
        public int ParentId { get; set; }
        [JsonProperty("parentType")]
        public MixEnums.MixAttributeSetDataType ParentType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion
        #region Views

        public MixAttributeSets.ReadMvcViewModel AttributeSet { get; set; }

        #endregion Views
        #endregion



        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSets.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == Id
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                AttributeSet = getData.Data;
            }
        }

        #endregion overrides
    }
}