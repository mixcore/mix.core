namespace Mix.Services.Permission.Domain.Dtos
{
    public class AddUserPermissionDto
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
        public string Description { get; set; }
    }
}
