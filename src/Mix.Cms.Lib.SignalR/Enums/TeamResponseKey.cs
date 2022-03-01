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
