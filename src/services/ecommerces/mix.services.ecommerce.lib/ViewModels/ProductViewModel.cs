using Microsoft.AspNetCore.Routing;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.Models;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using System.Threading;

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
            MixMetadataService metadataService, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            bool isChanged = false;
            if (AdditionalData == null)
            {
                AdditionalData = await ProductDetailsViewModel.GetRepository(ecommerceUow)
                    .GetSingleAsync(m =>
                        m.MixTenantId == MixTenantId &&
                        m.ParentId == Id &&
                        m.ParentType == MixDatabaseParentType.Post,
                        cancellationToken);
                isChanged = true;
            }

            if (PostMetadata == null)
            {
                var metadata = await metadataService.GetMetadataByContentId(Id, MetadataParentType.Post, string.Empty, new());
                PostMetadata = metadata.Items.Select(m => m.Metadata)
                .GroupBy(m => m.Type)
                .Select(m => new PostMetadata()
                {
                    MetadataType = m.Key,
                    Data = m.ToList()
                }).ToList();
                isChanged = true;
            }

            if (isChanged)
            {
                var cacheService = new MixCacheService();
                await cacheService.SetAsync($"{Id}/{GetType().FullName}", this, typeof(MixPostContent), "full");
            }
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
