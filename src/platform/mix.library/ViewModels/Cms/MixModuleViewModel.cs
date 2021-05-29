using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixModuleViewModel : ViewModelBase<MixCmsContext, MixModule, MixModuleViewModel>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public string FormTemplate { get; set; }
        public string EdmTemplate { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string PostType { get; set; }
        public int? PageSize { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixModuleViewModel() : base()
        {
        }

        public MixModuleViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
