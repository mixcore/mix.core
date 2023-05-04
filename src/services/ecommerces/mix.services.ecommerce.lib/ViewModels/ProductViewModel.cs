using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Models;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Newtonsoft.Json.Linq;

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

        public ProductDetailsViewModel ProductDetails { get; set; }
        public JObject AdditionalData { get; set; }
        public List<PostMetadata> PostMetadata { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method

        public async Task LoadAdditionalDataAsync(
            UnitOfWorkInfo<EcommerceDbContext> ecommerceUow,
            IMixMetadataService metadataService,
            MixCacheService cacheService,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool isChanged = false;
            if (ProductDetails == null)
            {
                CacheService ??= cacheService;
                ProductDetails = await ProductDetailsViewModel.GetRepository(ecommerceUow, CacheService)
                    .GetSingleAsync(m =>
                        m.MixTenantId == MixTenantId &&
                        m.ParentId == Id &&
                        m.ParentType == MixDatabaseParentType.Post,
                        cancellationToken);
                if (ProductDetails != null)
                {
                    AdditionalData = ReflectionHelper.ParseObject(ProductDetails);
                }
                isChanged = true;
            }

            if (PostMetadata == null)
            {
                var metadata = await metadataService.GetMetadataByContentId(Id, MixContentType.Post, string.Empty, new());
                PostMetadata = metadata.Items.Select(m => m.Metadata)
                .GroupBy(m => m.Type)
                .Select(m => new PostMetadata()
                {
                    MetadataType = m.Key,
                    Data = m.ToList()
                }).ToList();
                isChanged = true;
            }

            if (isChanged && CacheService != null)
            {
                await CacheService.SetAsync($"{Id}/{GetType().FullName}", this, typeof(MixPostContent).FullName, "full");
            }
        }
        #endregion

        #region Methods

        public T? Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        #endregion
    }
}
