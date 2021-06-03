using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixPagePostViewModel : ViewModelBase<MixCmsContext, MixPagePost, MixPagePostViewModel>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int PostId { get; set; }
        public int PageId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        #endregion Properties

        #region Contructors

        public MixPagePostViewModel() : base()
        {
        }

        public MixPagePostViewModel(MixPagePost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
