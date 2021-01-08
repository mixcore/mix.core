namespace Mix.Cms.Service.SignalR.Domain.Enums
{
    public enum MixTeamResponseKey
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
