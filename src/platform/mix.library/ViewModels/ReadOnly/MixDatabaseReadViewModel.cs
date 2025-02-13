using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Mix.Service.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Mix.Lib.ViewModels.ReadOnly
{
    public sealed class MixDatabaseReadViewModel : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, MixDatabaseReadViewModel>
    {
        #region Properties
        public int? MixDatabaseContextId { get; set; }
        [Required]
        public string SystemName { get; set; }
        public MixDatabaseType Type { get; set; }
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseReadViewModel()
        {
        }

        public MixDatabaseReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseReadViewModel(MixDatabase entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

    }
}
