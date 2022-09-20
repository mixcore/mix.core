using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.RepoDb.Repositories;
using Mix.Shared.Services;

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

        public PostContentViewModel(MixPostContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"{GlobalConfigService.Instance.Domain}/post/{Id}/{SeoName}";

        public JObject AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            await base.ExpandView();
            if (!Image.StartsWith("http") && !string.IsNullOrEmpty(GlobalConfigService.Instance.Domain))
            {
                Image = $"{GlobalConfigService.Instance.Domain}{Image}";
            }
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
            mixRepoDbRepository.Init(MixDatabaseName);
            var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Post, Id);
            AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
