namespace Mix.Cms.Lib.SignalR.Constants
{
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
}
