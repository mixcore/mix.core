using Mix.RepoDb.Repositories;

namespace Mix.Common.Domain.ViewModels
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

        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            if (AdditionalData == null)
            {
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Post, Id);
                var relationships = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == MixDatabaseName);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
                foreach (var item in relationships)
                {
                    mixRepoDbRepository.InitTableName(item.DestinateDatabaseName);
                    var arr = await mixRepoDbRepository.GetListByParentAsync(MixContentType.Post, obj.Id);
                    AdditionalData.Add(new JProperty(item.DisplayName, JArray.FromObject(arr)));
                }
                
            }
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
