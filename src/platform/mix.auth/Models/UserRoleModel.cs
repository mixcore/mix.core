namespace Mix.Auth.Models
{
    public sealed class UserRoleModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsUserInRole { get; set; }
    }
}
