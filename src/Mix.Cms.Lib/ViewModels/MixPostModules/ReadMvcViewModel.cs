using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixPostModules
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPostModule, ReadViewModel>
    {
        public ReadViewModel(MixPostModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("moduleId")]
        public int ModuleId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public MixEnums.MixContentStatus Status { get; set; }

        #region Views

        [JsonProperty("module")]
        public MixModules.ReadMvcViewModel Module { get; set; }

        #endregion Views

        #region overrides
        public override MixPostModule ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(m => m.Id, _context, _transaction).Data + 1;
            }
            return base.ParseModel(_context, _transaction);
        }
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getModule = MixModules.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == ModuleId && p.Specificulture == Specificulture
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