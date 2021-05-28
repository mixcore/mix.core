using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;
using System.Collections.Generic;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportDataAssociationViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseDataAssociation, ImportDataAssociationViewModel>
    {
        #region Properties

        #region Model

        /*
         * Attribute Set Data Id
         */

        public string Id { get; set; }

        public string Specificulture { get; set; }

        public string DataId { get; set; }

        public List<SupportedCulture> Cultures { get; set; }

        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        public string ParentId { get; set; }

        public MixDatabaseParentType ParentType { get; set; }

        public int MixDatabaseId { get; set; }

        public string MixDatabaseName { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Model

        #region Views

        public bool IsProcessed { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportDataAssociationViewModel() : base()
        {
        }

        public ImportDataAssociationViewModel(MixDatabaseDataAssociation model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}