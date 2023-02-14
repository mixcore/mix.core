using Mix.Lib.Models.Common;

namespace Mix.Lib.Interfaces
{
    public interface IMixDataService
    {
        public void SetUnitOfWork(UnitOfWorkInfo uow);

        public Task<List<TView>> GetByAllParent<TView>(SearchDataContentModel request, string culture = null)
            where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid, TView>;

        public Task<PagingResponseModel<TView>> Search<TView>(SearchDataContentModel searchRequest, string culture = null, CancellationToken cancellationToken = default)
            where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid, TView>;

        public Task LoadAllReferenceDataAsync<TView>(JObject obj, Guid dataContentId, string mixDatabaseName, List<MixDatabaseColumn> refColumns = null)
            where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, TView>;

        public Task<JArray> GetRelatedDataContentAsync<TView>(int referenceId, Guid dataContentId)
            where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, TView>;
    }
}
