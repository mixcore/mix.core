using Mix.Constant.Enums;
using Mix.Heart.Entities;

namespace Mix.Services.Ecommerce.Lib.Entities
{
    public class ProductDetails: EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public double? Price { get; set; }
        public string? DesignBy { get; set; }
        public string? Information { get; set; }
        public string? InformationImage { get; set; }
        public string? Size { get; set; }
        public string? SizeImage { get; set; }
        public string? Document { get; set; }
        public string? MaintenanceDocument { get; set; }
    }
}
