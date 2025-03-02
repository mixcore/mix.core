using Mix.Heart.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Models;
using RepoDb;
using RepoDb.Enumerations;

namespace Mixcore.Domain.ViewModels
{
    public class PostContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPostContent, int, PostContentViewModel>
    {
        #region Constructors

        public PostContentViewModel()
        {
        }

        public PostContentViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        public PostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"/post/{Id}/{SeoName}";

        public JObject AdditionalData { get; set; }
        public List<PostMetadata> PostMetadata { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method


        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        public async Task LoadAdditionalDataAsync(
                IMixDbDataService mixDbDataService,
                IMixMetadataService metadataService,
                MixCacheService cacheService, 
                CancellationToken cancellationToken)
        {
            bool isChanged = false;
            if (AdditionalData == null && !string.IsNullOrEmpty(MixDatabaseName))
            {
                isChanged = true;
                var relationships = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == MixDatabaseName).ToList();
                var obj = await mixDbDataService.GetSingleByParentAsync(MixDatabaseName, MixContentType.Post, Id, string.Empty, cancellationToken);
                if (obj != null)
                {
                    AdditionalData = ReflectionHelper.ParseObject(obj);
                }
            }
            if (PostMetadata == null)
            {

                isChanged = true;
                var metadata = await metadataService.GetMetadataByContentId(Id, MixContentType.Post, string.Empty, new());
                PostMetadata = metadata.Items.Select(m => m.Metadata)
                    .GroupBy(m => m.Type)
                    .Select(m => new PostMetadata()
                    {
                        MetadataType = m.Key,
                        Data = m.ToList()
                    }).ToList();
            }

            if (isChanged)
            {
                await cacheService.SetAsync($"{Id}/{GetType().Name}", this, $"{typeof(MixPostContent).Assembly.GetName().Name}_{typeof(MixPostContent).Name}", "full");
            }

        }
        #endregion

        #region Private Methods


        #endregion
    }
}
