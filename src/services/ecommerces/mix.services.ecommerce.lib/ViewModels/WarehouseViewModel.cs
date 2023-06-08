using Mix.Constant.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities;
using Mix.Services.Ecommerce.Lib.Entities.Mix;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class WarehouseViewModel : ViewModelBase<EcommerceDbContext, Warehouse, int, WarehouseViewModel>
    {
        #region Properties

        public int MixTenantId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int ProductDetailsId { get; set; }
        public string? Sku { get; set; }
        public double? Price { get; set; }
        public int? Inventory { get; set; }
        public int? Sold { get; set; }

        #endregion

        #region Contructors

        public WarehouseViewModel()
        {
        }

        public WarehouseViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public WarehouseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WarehouseViewModel(Warehouse entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
