using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Services.Ecommerce.Lib.Entities;

namespace Mix.Services.Ecommerce.Lib.ViewModels
{
    public class ProductViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPostContent, int, ProductViewModel>
    {
        #region Constructors

        public ProductViewModel()
        {
        }

        public ProductViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public ProductViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"/post/{Id}/{SeoName}";

        public ProductDetailsViewModel AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method

        public async Task LoadAdditionalDataAsync(UnitOfWorkInfo<EcommerceDbContext> ecommerceUow, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (AdditionalData == null)
            {
                AdditionalData = await ProductDetailsViewModel.GetRepository(ecommerceUow)
                    .GetSingleAsync(m => 
                        m.MixTenantId == MixTenantId &&
                        m.ParentId == Id && 
                        m.ParentType == MixDatabaseParentType.Post,
                        cancellationToken);

            }
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
