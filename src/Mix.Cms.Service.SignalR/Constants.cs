namespace Mix.Cms.Service.SignalR
{
    public class Constants
    {
        public const string DefaultDevice = "website";
       
        public class HubMessages
        {
            public const string HubMemberName = "hub_member";
            public const string HubMemberFieldName = "hub_name";
            public const string UnknowErrorMsg = "Unknow";


        }
        public class Enums
        {
           

            public enum MixApiResponseKey
            {
                Succeed,
                Failed
            }

         

           

            public enum MixMessageType
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
