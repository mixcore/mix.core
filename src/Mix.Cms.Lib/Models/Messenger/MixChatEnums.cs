namespace Mix.Cms.Messenger
{
    public class MixChatEnums
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
            SendMessage,
            GetTeamNotifications,
            NotMembered,
            CancelRequest,
            RejectInvite,
            Error
        }

        public enum ApiResponseKey
        {
            Succeed,
            Failed
        }

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
            NotAuthorized,
            TeamFulled
        }

        public enum OnlineStatus
        {
            DisConnected = 0,
            Connected = 1
        }

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

        public enum NotificationType
        {
            NewMessage = 0,
            Join = 1,
            Left = 2
        }

        public enum DeviceStatus
        {
            Deactived = 0,
            Actived = 1,
            Banned = 2,
            Disconnected = 3
        }
    }
}