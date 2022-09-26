using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Universal.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.ViewModels
{
    public class OrganizationViewModel : ViewModelBase<MixUniversalDbContext, Organization, int, OrganizationViewModel>
    {
        #region Properties

        public string Title { get; set; }
        public string Description { get; set; }
        public string Endpoint { get; set; }
        public int? TenantId { get; set; }

        public List<PortalAppViewModel> PortalApps { get; set; }
        #endregion

        #region Contructors

        public OrganizationViewModel()
        {
        }

        public OrganizationViewModel(MixUniversalDbContext context) : base(context)
        {
        }

        public OrganizationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OrganizationViewModel(Organization entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            PortalApps = await PortalAppViewModel.GetRepository(UowInfo).GetListAsync(m => m.TenantId == TenantId && m.OrganizationId == Id);
        }

        protected override async Task SaveEntityRelationshipAsync(Organization parentEntity)
        {
            
            if (PortalApps != null && PortalApps.Count > 0)
            {
                List<Task> tasks = new();
                foreach (var app in PortalApps)
                {
                    app.OrganizationId = parentEntity.Id;
                    app.TenantId = TenantId;
                    app.SetUowInfo(UowInfo);
                    tasks.Add(app.SaveAsync());
                }
                await Task.WhenAll(tasks);
            }
        }

        #endregion
    }
}
