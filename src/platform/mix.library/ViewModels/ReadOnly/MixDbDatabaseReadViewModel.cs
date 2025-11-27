using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels.ReadOnly
{
    public sealed class MixDbDatabaseReadViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDbDatabase, int, MixDbDatabaseReadViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        #endregion

        #region Constructors

        public MixDbDatabaseReadViewModel()
        {

        }

        public MixDbDatabaseReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbDatabaseReadViewModel(MixDbDatabase entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
