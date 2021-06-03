using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixModulePostViewModel : ViewModelBase<MixCmsContext, MixModulePost, MixModulePostViewModel>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int PostId { get; set; }
        public int ModuleId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        public MixModuleViewModel Module { get; set; }
        public MixPostViewModel Post { get; set; }

        #endregion Properties

        #region Contructors

        public MixModulePostViewModel() : base()
        {
        }

        public MixModulePostViewModel(MixModulePost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #endregion Overrides

        #region Expand

        public async Task LoadModuleAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getmodule = await MixModuleViewModel.Repository.GetSingleModelAsync(
                m => m.Specificulture == Specificulture && m.Id == ModuleId);
            Module = getmodule.Data;
        }
        
        public async Task LoadPostAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = await MixPostViewModel.Repository.GetSingleModelAsync(
                m => m.Specificulture == Specificulture && m.Id == PostId);
            Post = getPost.Data;
        }

        #endregion Expand
    }
}
