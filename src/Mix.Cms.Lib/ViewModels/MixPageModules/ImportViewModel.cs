using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPageModules
{
    public class ImportViewModel
       : ViewModelBase<MixCmsContext, MixPageModule, ImportViewModel>
    {
        public ImportViewModel(MixPageModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ImportViewModel() : base()
        {
        }

        [JsonProperty("moduleId")]
        public int ModuleId { get; set; }

        [JsonProperty("pageId")]
        public int PageId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        [JsonProperty("module")]
        public MixModules.ImportViewModel Module { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getModule = MixModules.ImportViewModel.Repository.GetSingleModel(p => p.Id == ModuleId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getModule.IsSucceed)
            {
                Module = getModule.Data;
            }
        }

        #endregion overrides
    }
}