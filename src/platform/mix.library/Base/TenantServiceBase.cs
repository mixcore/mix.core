using Microsoft.AspNetCore.Http;
using Mix.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Base
{
    public abstract class TenantServiceBase
    {
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        protected IHttpContextAccessor _httpContextAccessor;

        protected TenantServiceBase(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _httpContextAccessor = httpContextAccessor;
            _cmsUOW = cmsUOW;
        }

        private MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _httpContextAccessor.HttpContext.Session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
    }
}
