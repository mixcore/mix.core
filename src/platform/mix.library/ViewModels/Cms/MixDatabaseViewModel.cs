using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixDatabaseViewModel : ViewModelBase<MixCmsContext, MixDatabase, MixDatabaseViewModel>
    {
        #region Properties
        public int Id { get; set; }
        public MixDatabaseType Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormTemplate { get; set; }
        public string EdmTemplate { get; set; }
        public string EdmSubject { get; set; }
        public string EdmFrom { get; set; }
        public bool? EdmAutoSend { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixDatabaseViewModel() : base()
        {
        }

        public MixDatabaseViewModel(MixDatabase model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
