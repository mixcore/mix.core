using Newtonsoft.Json;

namespace Mix.Identity.Models.AccountViewModels
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsUserInRole { get; set; }
    }
}