using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;
using System.Linq;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixModuleDataViewModel : ViewModelBase<MixCmsContext, MixModuleData, MixModuleDataViewModel>
    {
        #region Properties

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int ModuleId { get; set; }
        public int? PageId { get; set; }
        public int? PostId { get; set; }
        public string Fields { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        #endregion Properties

        #region Contructors

        public MixModuleDataViewModel() : base()
        {
        }

        public MixModuleDataViewModel(MixModuleData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override MixModuleData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Fields = _context.MixModule.First(m => m.Id == ModuleId && m.Specificulture == Specificulture)?.Fields;
        }
        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
