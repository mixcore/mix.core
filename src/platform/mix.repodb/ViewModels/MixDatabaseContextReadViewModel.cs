using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Base;
using System.ComponentModel.DataAnnotations;

namespace Mix.RepoDb.ViewModels
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
