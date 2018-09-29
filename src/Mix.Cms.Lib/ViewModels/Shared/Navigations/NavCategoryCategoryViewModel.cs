using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.Shared
{
    public class NavCategoryCategoryViewModel
       : ViewModelBase<MixCmsContext, MixCategoryCategory, NavCategoryCategoryViewModel>
    {
        public NavCategoryCategoryViewModel(MixCategoryCategory model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public NavCategoryCategoryViewModel() : base()
        {
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public Page.ReadListItemViewModel Category { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCategory = Page.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == ParentId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getCategory.IsSucceed)
            {
                Category = getCategory.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
