using Mix.Constant.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities;
using Mix.Services.Ecommerce.Lib.Entities.Mix;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class ProductVariantViewModel : ViewModelBase<EcommerceDbContext, ProductVariant, int, ProductVariantViewModel>
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

        public ProductVariantViewModel()
        {
        }

        public ProductVariantViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public ProductVariantViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public ProductVariantViewModel(ProductVariant entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
