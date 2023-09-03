using Mix.Constant.Enums;
using Mix.Heart.Models;
using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Models;
using Mix.Shared.Dtos;
using Mix.Shared.Models;

namespace Mix.Services.Databases.Lib.Interfaces
{
    public interface IMixMetadataService
    {
        public Task<List<PostMetadata>> GetMetadataAsync(string[]? includes = null, string[]? excepts = null);

        public Task<MixMetadataViewModel> GetOrCreateMetadata(CreateMetadataDto dto, CancellationToken cancellationToken = default);

        public Task<MixMetadataViewModel> CreateMetadata(CreateMetadataDto dto, CancellationToken cancellationToken = default);

        public Task CreateMetadataContentAssociation(CreateMetadataContentAssociationDto dto, CancellationToken cancellationToken = default);

        public Task<PagingResponseModel<MixMetadataContentAsscociationViewModel>?> GetMetadataByContentId(int intContentId, MixContentType? contentType, string metadataType, PagingRequestModel pagingData);

        public Task DeleteMetadataContentAssociation(int id, CancellationToken cancellationToken = default);

        public IQueryable<int>? GetQueryableContentIdByMetadataSeoContent(List<SearchQueryField> metadataSeoContents, MixContentType contentType);

        public IQueryable<int>? GetQueryableMetadataByContentId(int contentId, MixContentType? contentType, string metadataType);
    }
}
