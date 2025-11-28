using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Identity.Dtos
{
    public sealed class GrantPermissionsDto
    {
        public List<PermissionDto> Permissions { get; set; }
        public bool IsActive { get; set; }
        public string? RequestedBy { get; set; }
    }
    public sealed class PermissionDto
    {
        public Guid RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
