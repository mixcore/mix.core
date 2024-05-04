﻿using Mix.Lib.Interfaces;

namespace Mix.SignalR.Hubs
{
    public class MixThemeHub : BaseSignalRHub
    {
        public MixThemeHub(IAuditLogService auditLogService, IMixTenantService mixTenantService)
            : base(auditLogService, mixTenantService)
        {
        }
    }
}