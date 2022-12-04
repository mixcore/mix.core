using Mix.Constant.Enums;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Dtos
{
    public class CreateUserAddressDto
    {
        public string? Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Note { get; set; }
    }
}
