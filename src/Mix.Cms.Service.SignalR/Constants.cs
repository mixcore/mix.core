using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Service.SignalR
{
    public class Constants
    {
        public const string DefaultDevice = "website";
        public class HubMethods
        {
            public const string ReceiveMethod = "receive_message";
            public const string SendMessage = "send_message";
            public const string SendGroupMessage = "send_group_message";
            public const string SendPrivateMessage = "send_private_message";
            public const string SaveData = "save_data";
            public const string JoinGroup = "join_group";
            public const string NewMessage = "new_message";
            public const string NewNotification = "new_notification";
            public const string NewMember = "new_member";
        }
        public class HubMessages
        {
            public const string HubMemberName = "hub_member";
            public const string HubMemberFieldName = "hub_name";            
            public const string UnknowErrorMsg = "Unknow";

            
        }
        public class Enums
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
                UnAuthorized,
                TeamFulled
            }

            public enum OnlineStatus
            {
                Disconnected = 0,
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
}
