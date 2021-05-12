using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Enums
{
    public enum MessageType
    {
        String = 0,
        Notification = 1,
        Image = 2,
        File = 3,
        Voice = 4,
        Location = 5,
        Html = 6
    }
}
