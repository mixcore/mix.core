using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Runtime;
using Mix.Shared.Services;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
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

        public JArray AdditionalData{ get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            await base.ExpandView();
        }



        #endregion

        #region Public Method


        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        public Task LoadAdditionalDataAsync(RuntimeDbContextService runtimeDbContextService)
        {
            var repo = runtimeDbContextService.GetRepository(MixDatabaseName);
            AdditionalData = JArray.FromObject(repo.GetListByParent(Id));
            return Task.CompletedTask;
        }
        #endregion
        #region Private Methods


        #endregion
    }
}
