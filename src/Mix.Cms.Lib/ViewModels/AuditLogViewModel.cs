using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.NetCore.Attributes;
using System;
using System.Runtime.Serialization;

namespace Mix.Cms.Lib.ViewModels
{
    [GeneratedController("api/v1/rest/audit-log")]
    public class AuditLogViewModel:ViewModelBase<AuditContext, AuditLog, AuditLogViewModel>
    {
        public AuditLogViewModel()
        {
        }

        public AuditLogViewModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AuditLogViewModel(AuditLog model, AuditContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        public Guid Id { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }
        public bool Success { get; set; }
        public string Exception { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
