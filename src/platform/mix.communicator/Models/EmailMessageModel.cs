using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Communicator.Models
{
    public sealed class EmailMessageModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string To { get; set; }
        public string? From { get; set; }
    }
}
