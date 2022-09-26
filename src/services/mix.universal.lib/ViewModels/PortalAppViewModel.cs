using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Universal.Lib.Entities;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.ViewModels
{
    [GenerateRestApiController]
    public class PortalAppViewModel : ViewModelBase<MixUniversalDbContext, PortalApp, int, PortalAppViewModel>
    {
        #region Properties

        public string Title { get; set; }
        public string Path { get; set; }
        public string MixcoreVersion { get; set; }
        public int OrganizationId { get; set; }
        public int? TenantId { get; set; }

        #endregion

        #region Contructors

        public PortalAppViewModel()
        {
        }

        public PortalAppViewModel(MixUniversalDbContext context) : base(context)
        {
        }

        public PortalAppViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public PortalAppViewModel(PortalApp entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
