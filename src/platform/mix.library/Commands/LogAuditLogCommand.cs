using Microsoft.AspNetCore.Http;
using Mix.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Commands
{
    public class LogAuditLogCommand
    {
        public LogAuditLogCommand()
        {

        }
        public LogAuditLogCommand(string userName, HttpRequest request, Exception ex = null)
        {
            UserName = userName;
            Request = new(request);
            Exception = ex;
        }
        public string UserName { get; set; }
        public ParsedRequestModel Request { get; set; }
        public Exception Exception { get; set; }

        
    }
}
