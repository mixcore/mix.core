using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Enums
{
    public enum TeamResponseKey
    {
        GetTeamsSucceed,
        GetTeamsFailed,

        SaveTeamSucceed,
        SaveTeamFailed,
        InvalidModel,
        NameExisted,
        NameRequired,
        CountryRequired,

        SearchTeamMembersSucceed,
        SearchTeamMembersFailed,

        SetMemberStatusSucceed,
        SetMemberStatusFailed,
        UnAuthorized,
        TeamFulled
    }
}
