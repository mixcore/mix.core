using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class OrderTrackingViewModel : ViewModelBase<EcommerceDbContext, OrderTracking, int, OrderTrackingViewModel>
    {
        #region Properties

        public OrderTrackingAction? Action { get; set; }
        public string? Note { get; set; }
        public int OrderDetailId { get; set; }
        public int TenantId { get; set; }

        #endregion

        #region Contructors

        public OrderTrackingViewModel()
        {
            IsCache = false;
        }

        public OrderTrackingViewModel(EcommerceDbContext context) : base(context)
        {
            IsCache = false;
        }

        public OrderTrackingViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
            IsCache = false;
        }

        public OrderTrackingViewModel(OrderTracking entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
            IsCache = false;
        }

        #endregion
    }
}
