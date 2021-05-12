using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Enums
{
    public enum MemberStatus
    {
        Requested = 0,
        Invited = 1,
        AdminRejected = 2,
        MemberRejected = 3,
        Banned = 4,
        Membered = 5,
        AdminRemoved = 6,
        MemberCanceled = 7,
        Guest = 8,
        MemberAccepted = 9,
        MemberLeft = 10
    }
}
