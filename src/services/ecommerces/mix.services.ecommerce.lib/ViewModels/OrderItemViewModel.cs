using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Payments.Lib.Constants;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    [GenerateRestApiController]
    public class OrderItemViewModel : ViewModelBase<EcommerceDbContext, OrderItem, int, OrderItemViewModel>
    {
        #region Properties
        public string Sku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ReferenceUrl { get; set; }
        public string Currency { get; set; }
        public int PostId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public int OrderDetailId { get; set; }
        public int MixTenantId { get; set; }

        public bool IsActive { get; set; }

        #endregion

        #region Contructors

        public OrderItemViewModel()
        {
            IsCache = false;
        }

        public OrderItemViewModel(EcommerceDbContext context) : base(context)
        {
            IsCache = false;
        }

        public OrderItemViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
            IsCache = false;
        }

        public OrderItemViewModel(OrderItem entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
            IsCache = false;
        }

        #endregion

        #region Overrides

        public override Task<OrderItem> ParseEntity(CancellationToken cancellationToken = default)
        {
            Calculate();
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(OrderItem parentEntity, CancellationToken cancellationToken = default)
        {
            MixDatabaseAssociationViewModel association = await GetCurrentAssociation();
            if (association == null)
            {
                association = new(UowInfo)
                {
                    ParentDatabaseName = EcommerceConstants.DataTableNameOrder,
                    ParentId = parentEntity.OrderDetailId,
                    ChildId = parentEntity.Id,
                    ChildDatabaseName = EcommerceConstants.DataTableNameOrderItem,
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = CreatedBy,
                    MixTenantId = MixTenantId
                };
                await association.SaveAsync(cancellationToken);
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            await base.DeleteHandlerAsync(cancellationToken);
            MixDatabaseAssociationViewModel association = await GetCurrentAssociation();
            if (association != null)
            {
                await association.DeleteAsync(cancellationToken);
            }

        }

        private async Task<MixDatabaseAssociationViewModel> GetCurrentAssociation()
        {
            return await MixDatabaseAssociationViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(m =>
                        m.ParentId == OrderDetailId
                        && m.ChildId == Id
                        && m.ChildDatabaseName == EcommerceConstants.DataTableNameOrderItem);
        }
        #endregion

        #region Methods

        public void Calculate()
        {
            Price = Context.ProductVariant.SingleOrDefault(m => m.Sku == Sku)?.Price ?? 0;
            Total = IsActive ? Price * Quantity : 0;
        }
        #endregion
    }
}
