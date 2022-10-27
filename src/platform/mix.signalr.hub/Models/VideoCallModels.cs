using Mix.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Signalr.Hub.Models
{
    public class CallOffer
    {
        public User Caller;
        public User Callee;
    }
    public class User
    {
        public string Username;
        public string ConnectionId;
        public bool InCall;
    }
    public class UserCall
    {
        public List<User> Users;
    }
}
