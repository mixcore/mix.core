using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using System;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDataAssociations
{
    public class DeleteViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseDataAssociation, DeleteViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string DataId { get; set; }
        public string ParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int MixDatabaseId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabaseDataAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixDatabaseDataAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}