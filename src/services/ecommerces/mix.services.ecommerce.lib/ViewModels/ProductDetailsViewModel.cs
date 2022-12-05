using Mix.Constant.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class ProductDetailsViewModel: ViewModelBase<EcommerceDbContext, ProductDetails, int, ProductDetailsViewModel>
    {
        #region Properties

        public int MixTenantId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public double? Price { get; set; }
        public ProductMetadata Metadata { get; set; } = new();

        #endregion

        #region Contructors

        public ProductDetailsViewModel()
        {
        }

        public ProductDetailsViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public ProductDetailsViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public ProductDetailsViewModel(ProductDetails entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
