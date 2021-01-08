using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class DeleteViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseContentAssociation, DeleteViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string DataId { get; set; }
        public string ParentId { get; set; }
        public MixDatabaseContentAssociationType ParentType { get; set; }
        public int AttributeSetId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixDatabaseContentAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixDatabaseContentAssociation ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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