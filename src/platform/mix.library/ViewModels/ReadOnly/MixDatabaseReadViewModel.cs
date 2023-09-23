using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels.ReadOnly
{
    public sealed class MixDatabaseContextReadViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabaseContext, int, MixDatabaseContextReadViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseContextReadViewModel()
        {

        }

        public MixDatabaseContextReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseContextReadViewModel(MixDatabaseContext entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
