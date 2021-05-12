using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixPortalPageRoles
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPortalPageRole, ReadViewModel>
    {
        #region Properties

        #region Model

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("roleId")]
        public string RoleId { get; set; }

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

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        //[JsonProperty("page")]
        //public MixPortalPages.ReadViewModel Page { get; set; }

        #endregion Views

        #endregion Properties

        public ReadViewModel(MixPortalPageRole model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }

        #region overrides

        public override MixPortalPageRole ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //var getCategory = MixPortalPages.ReadViewModel.Repository.GetSingleModel(p => p.Id == Id
            //, _context: _context, _transaction: _transaction
            //);
            //if (getCategory.IsSucceed)
            //{
            //    Page = getCategory.Data;
            //}
        }

        #endregion overrides
    }
}