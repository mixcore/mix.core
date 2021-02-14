using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseAssociations
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, Models.Cms.MixDatabaseAssociation, ReadMvcViewModel>
    {
        public ReadMvcViewModel(Models.Cms.MixDatabaseAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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

        [JsonProperty("mixDatabaseid")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseParentType ParentType { get; set; }

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

        #endregion Models

        #region Views

        public MixDatabases.ReadMvcViewModel MixDatabase { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixDatabases.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == Id
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                MixDatabase = getData.Data;
            }
        }

        #endregion overrides
    }
}