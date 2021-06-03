using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixUrlAliasViewModel : ViewModelBase<MixCmsContext, MixUrlAlias, MixUrlAliasViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public string SourceId { get; set; }

        public MixUrlAliasType Type { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public MixUrlAliasViewModel() : base()
        {
        }

        public MixUrlAliasViewModel(MixUrlAlias model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixUrlAlias ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (Repository.CheckIsExists(o =>
                    o.Alias == Alias && o.Specificulture == Specificulture && o.Id != Id, _context, _transaction))
                {
                    Errors.Add("Alias Existed");
                    IsValid = false;
                }
            }
        }

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
