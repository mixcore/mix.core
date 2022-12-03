namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateUserPermissionDto
    {
        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
        public string Description { get; set; }
    }
}
