﻿using Microsoft.AspNetCore.Http;
using Mix.Log.Lib.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Log.Lib.Interfaces
{
    public interface IAuditLogService
    {
        public Task SaveRequestAsync(AuditLogDataModel request);
        public void QueueRequest(AuditLogDataModel request);
        Task LogStream(string? message, object? data = null, Exception? ex = null, bool isSuccess = false);
    }
}
