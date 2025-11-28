using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Mix.Service.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Mix.Lib.ViewModels.ReadOnly
{
    public sealed class MixDbTableReadViewModel : TenantDataViewModelBase<MixCmsContext, MixDbTable, int, MixDbTableReadViewModel>
    {
        #region Properties
        public int? MixDbDatabaseId { get; set; }
        [Required]
        public string SystemName { get; set; }
        public MixDbTableType Type { get; set; }
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }
        #endregion

        #region Constructors

        public MixDbTableReadViewModel()
        {
        }

        public MixDbTableReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbTableReadViewModel(MixDbTable entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

    }
}
