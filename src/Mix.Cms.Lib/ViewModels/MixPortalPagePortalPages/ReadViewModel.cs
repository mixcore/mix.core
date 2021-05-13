using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixPortalPagePortalPages
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPortalPageNavigation, ReadViewModel>
    {
        public ReadViewModel(MixPortalPageNavigation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        #region Properties

        #region Models

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

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

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("page")]
        public MixPortalPages.ReadViewModel Page { get; set; }

        [JsonProperty("parent")]
        public MixPortalPages.ReadViewModel Parent { get; set; }

        #endregion Views

        #endregion Properties

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Page = MixPortalPages.ReadViewModel.Repository.GetSingleModel(p => p.Id == PageId).Data;
            Parent = MixPortalPages.ReadViewModel.Repository.GetSingleModel(p => p.Id == ParentId).Data;
        }

        #endregion overrides
    }
}