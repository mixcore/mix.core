using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixModuleViewModel : MixModuleViewModelBase<MixModuleViewModel>
    {
        #region Properties


        

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
