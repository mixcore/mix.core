using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Enums
{
    public enum MessageReponseKey
    {
        NewMessage,
        NewGroupMessage,
        NewInvite,
        NewRequest,
        RemovedTeam,
        RemovedMember,
        MemberOffline,
        NewTeam,
        NewMember,
        Connect,
        ConnectSuccess,
        ConnectFailed,
        ConnectInitData,
        GetTeam,
        GetTeamMessages,
        PreviousMessages,
        SendMessage,
        GetTeamNotifications,
        NotMembered,
        CancelRequest,
        RejectInvite,
        Error
    }
}
