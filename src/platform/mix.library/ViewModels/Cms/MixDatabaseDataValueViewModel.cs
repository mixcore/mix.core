using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixDatabaseDataValueViewModel : ViewModelBase<MixCmsContext, MixDatabaseDataValue, MixDatabaseDataValueViewModel>
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixDatabaseDataValueViewModel() : base()
        {
        }

        public MixDatabaseDataValueViewModel(MixDatabaseDataValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
