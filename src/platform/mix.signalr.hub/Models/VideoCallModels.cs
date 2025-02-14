namespace Mix.Signalr.Hub.Models
{
    public class CallOffer
    {
        public User Caller;
        public User Callee;
    }
    public class User
    {
        public string UserName;
        public string ConnectionId;
        public bool InCall;
    }
    public class UserCall
    {
        public List<User> Users;
    }
}
