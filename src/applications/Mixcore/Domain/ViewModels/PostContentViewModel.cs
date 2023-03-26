using Mix.Heart.Helpers;
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

        public PostContentViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
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
                MixRepoDbRepository mixRepoDbRepository, 
                IMixMetadataService metadataService,
                MixCacheService cacheService)
        {
            bool isChanged = false;
            if (AdditionalData == null && !string.IsNullOrEmpty(MixDatabaseName))
            {
                isChanged = true;
                var relationships = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == MixDatabaseName).ToList();
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Post, Id);
                if (obj != null)
                {
                    AdditionalData = ReflectionHelper.ParseObject(obj);

                    foreach (var item in relationships)
                    {

                        mixRepoDbRepository.InitTableName(item.DestinateDatabaseName);
                        var allowsIds = Context.MixDatabaseAssociation
                                .Where(m => m.ParentDatabaseName == MixDatabaseName
                                            && m.ParentId == AdditionalData.Value<int>("id")
                                            && m.ChildDatabaseName == item.DestinateDatabaseName)
                                .Select(m => m.ChildId).ToList();
                        var queries = new List<QueryField>()
                    {
                        new QueryField("Id", Operation.In, allowsIds)
                    };
                        var data = await mixRepoDbRepository.GetListByAsync(queries);
                        var arr = new JArray();
                        if (data != null)
                        {
                            foreach (var dataItem in data)
                            {
                                arr.Add(ReflectionHelper.ParseObject(dataItem));
                            }
                        }
                        AdditionalData.Add(new JProperty(item.DisplayName, JArray.FromObject(arr)));
                    }
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
                await cacheService.SetAsync($"{Id}/{GetType().FullName}", this, typeof(MixPostContent).FullName, "full");
            }

        }
        #endregion

        #region Private Methods


        #endregion
    }
}
