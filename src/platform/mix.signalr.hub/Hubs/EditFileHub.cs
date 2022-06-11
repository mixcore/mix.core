using Microsoft.AspNetCore.SignalR;
using Mix.Lib.Services;
using Mix.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    public class EditFileHub : BaseSignalRHub
    {
        public EditFileHub(AuditLogService auditLogService) : base(auditLogService)
        {
        }
    }
}