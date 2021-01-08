using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets
{
    public class DeleteViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseAssociation, DeleteViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }
        public int AttributeSetId { get; set; }
        public int ParentId { get; set; }
        public MixDatabaseContentAssociationType ParentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabaseAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixDatabaseAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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