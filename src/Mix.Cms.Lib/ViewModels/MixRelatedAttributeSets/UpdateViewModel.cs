using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixDatabaseAssociation, UpdateViewModel>
    {
        #region Properties

        #region Models
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }
        [JsonProperty("parentId")]
        public int ParentId { get; set; }
        [JsonProperty("parentType")]
        public MixDatabaseContentAssociationType ParentType { get; set; }
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
        public MixContentStatus Status { get; set; }
        #endregion

        #region Views

        public MixAttributeSets.UpdateViewModel Data { get; set; }

        #endregion Views

        #endregion Properties

        public UpdateViewModel(MixDatabaseAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        #region overrides

        public override MixDatabaseAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(p => p.Id == AttributeSetId
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                Data = getData.Data;
            }
        }

        #endregion overrides
    }
}