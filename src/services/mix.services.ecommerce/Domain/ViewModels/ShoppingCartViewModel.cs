using mix.services.ecommerce.Domain.Entities;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;

namespace mix.services.ecommerce.Domain.ViewModels
{
    [GenerateRestApiController]
    public class ShoppingCartViewModel : ViewModelBase<EcommerceDbContext, ShoppingCart, int, ShoppingCartViewModel>
    {
        #region Properties

        public Guid UserId { get; set; }
        public int SysUserDataId { get; set; }
        public string Title { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public ShoppingCartViewModel()
        {
        }

        public ShoppingCartViewModel(EcommerceDbContext context) : base(context)
        {
        }

        public ShoppingCartViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public ShoppingCartViewModel(ShoppingCart entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion
    }
}
