using Mix.Mixdb.Interfaces;
using Mix.RepoDb.Repositories;
using RepoDb;
using RepoDb.Enumerations;

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

        public async Task LoadAdditionalDataAsync(IMixDbDataService mixDbDataService, CancellationToken cancellationToken = default)
        {
            if (AdditionalData == null)
            {
                var obj = await mixDbDataService.GetSingleByParentAsync(MixDatabaseName, MixContentType.Post, Id, string.Empty, cancellationToken);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
            }
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
