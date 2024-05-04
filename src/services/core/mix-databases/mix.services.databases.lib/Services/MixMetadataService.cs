using Microsoft.AspNetCore.Http;
using Mix.Auth.Constants;
using Mix.Constant.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Helpers;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Models;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixMetadataService : TenantServiceBase, IMixMetadataService
    {
        private readonly MixIdentityService _identityService;
        private readonly UnitOfWorkInfo<MixDbDbContext> _uow;

        public MixMetadataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixDbDbContext> uow,
            MixIdentityService identityService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _identityService = identityService;
        }


        public async Task<List<PostMetadata>> GetMetadataAsync(string[]? includes = null, string[]? excepts = null)
        {
            Expression<Func<MixMetadata, bool>> predicate = m => m.MixTenantId == CurrentTenant.Id;
            predicate = predicate.AndAlsoIf(includes != null, m => includes!.Contains(m.Type));
            predicate = predicate.AndAlsoIf(excepts != null, m => !excepts!.Contains(m.Type));
            var data = await MixMetadataViewModel.GetRepository(_uow, CacheService).GetAllAsync(predicate);
            return data.GroupBy(m => m.Type)
                    .Select(m => new PostMetadata()
                    {
                        MetadataType = m.Key!,
                        Data = m.ToList()
                    }).ToList();
        }

        //public async Task<PagingResponseModel<TView>?> GetPostByMetadataAsync<TView, TEntity>(
        //        string[] metadataSeoContents,
        //        MetadataParentType contentType,
        //        HttpRequest request)
        //    where TView : ViewModelBase<MixCmsContext, MixPostContent, int, TView>
        //     where TEntity : EntityBase<int>
        //{
        //    SearchQueryModel<TEntity, int> searchRequest = new(request);
        //    var query = GetQueryableContentIdByMetadataSeoContent(metadataSeoContents, contentType);
        //    if (query != null)
        //    {
        //        // TODO: Share dbcontext transaction
        //        var allowIds = await query.ToListAsync();
        //        var repo = new Repository<MixCmsContext, MixPostContent, int, TView>(_cmsUow);
        //        return await repo.GetPagingAsync(m => m.MixTenantId == CurrentTenant.Id
        //                        && allowIds.Any(n => n == m.Id),
        //                        searchRequest.PagingData);
        //    }
        //    return default;
        //}

        public async Task<MixMetadataViewModel> GetOrCreateMetadata(CreateMetadataDto dto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var metadata = await MixMetadataViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.Content == dto.Content && m.Type == dto.Type, cancellationToken);
            if (metadata == null)
            {
                return await CreateMetadata(dto, cancellationToken);
            }
            return metadata;
        }

        public async Task<MixMetadataViewModel> CreateMetadata(CreateMetadataDto dto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            MixMetadataViewModel metadata = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.Map(dto, metadata);
            await metadata.SaveAsync(cancellationToken);
            return metadata;
        }

        public async Task CreateMetadataContentAssociation(CreateMetadataContentAssociationDto dto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            MixMetadataContentAsscociationViewModel association = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.Map(dto, association);
            await association.SaveAsync(cancellationToken);
        }

        public async Task<PagingResponseModel<MixMetadataContentAsscociationViewModel>?> GetMetadataByContentId(
            int intContentId,
            MixContentType? contentType,
            string metadataType, PagingRequestModel pagingData)
        {
            var query = GetQueryableMetadataByContentId(intContentId, contentType, metadataType);
            if (query != null)
            {
                return await MixMetadataContentAsscociationViewModel.GetRepository(_uow, CacheService)
                            .GetPagingAsync(
                                m => m.MixTenantId == CurrentTenant.Id
                                        && query.Any(n => n == m.Id),
                                        pagingData);
            }
            return default;
        }

        public async Task DeleteMetadataContentAssociation(int id, CancellationToken cancellationToken = default)
        {
            var association = await MixMetadataContentAsscociationViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.Id == id, cancellationToken);
            if (association != null)
            {
                await association.DeleteAsync(cancellationToken);
            }
        }

        #region IQueryables

        public IQueryable<int>? GetQueryableContentIdByMetadataSeoContent(List<SearchQueryField> metadataSeoContents, MixContentType contentType)
        {
            //Expression<Func<MixMetadata, bool>>? predicate = m => isMandatory;
            if (metadataSeoContents.Count == 0)
            {
                return default;
            }

            IQueryable<int>? andQueryIds = null;
            IQueryable<int>? orQueryIds = null;
            List<SearchQueryField> andQueries = metadataSeoContents.Where(m => m.IsRequired).ToList();
            List<SearchQueryField> orQueries = metadataSeoContents.Where(m => !m.IsRequired).ToList();
            foreach (var item in andQueries)
            {
                if (item.Value is null)
                {
                    continue;
                }

                Expression<Func<MixMetadata, bool>> predicate = m =>
                    m.Type == item.FieldName;


                if (item.CompareOperator == MixCompareOperator.InRange || item.CompareOperator == MixCompareOperator.NotInRange)
                {
                    var array = item.Value.ToString()!.Split(',');
                    predicate = predicate
                        .AndAlsoIf(item.CompareOperator == MixCompareOperator.InRange, m => array.Contains(m.SeoContent));

                    predicate = predicate
                        .AndAlsoIf(item.CompareOperator == MixCompareOperator.NotInRange, m => !array.Contains(m.SeoContent));
                }
                else
                {
                    predicate = predicate
                        .AndAlso(ReflectionHelper.GetExpression<MixMetadata>("SeoContent", item.Value.ToString(), MixCmsHelper.ParseExpressionMethod(item.CompareOperator)));
                }

                var allowMetadata = _uow.DbContext.MixMetadata.Where(predicate);
                var allowedContentIds = from metadata in allowMetadata
                                        join association in _uow.DbContext.MixMetadataContentAssociation
                                        on metadata.Id equals association.MetadataId
                                        where association.MixTenantId == CurrentTenant.Id && association.ContentType == contentType
                                        select association.ContentId;

                if (andQueryIds == null)
                {
                    andQueryIds = allowedContentIds.Distinct();
                }
                else
                {
                    andQueryIds = andQueryIds.Where(m => allowedContentIds.Contains(m));
                }
            }

            foreach (var item in orQueries)
            {
                if (item.Value == null)
                {
                    continue;
                }

                Expression<Func<MixMetadata, bool>> predicate = m =>
                    m.Type == item.FieldName;

                if (item.CompareOperator == MixCompareOperator.InRange || item.CompareOperator == MixCompareOperator.NotInRange)
                {
                    var array = item.Value.ToString()!.Split(',');
                    predicate = predicate.AndAlsoIf(item.CompareOperator == MixCompareOperator.InRange,
                                                    m => array.Contains(m.SeoContent));
                    predicate = predicate.AndAlsoIf(item.CompareOperator == MixCompareOperator.NotInRange,
                                                    m => !array.Contains(m.SeoContent));
                }
                else
                {
                    predicate = predicate.AndAlso(ReflectionHelper.GetExpression<MixMetadata>("SeoContent", item.Value.ToString(), MixCmsHelper.ParseExpressionMethod(item.CompareOperator)));
                }

                var allowMetadata = _uow.DbContext.MixMetadata.Where(predicate);

                var allowedContentIds = from metadata in allowMetadata
                                        join association in _uow.DbContext.MixMetadataContentAssociation
                                        on metadata.Id equals association.MetadataId
                                        where association.MixTenantId == CurrentTenant.Id && association.ContentType == contentType
                                        select association.ContentId;
                if (orQueryIds == null)
                {
                    orQueryIds = allowedContentIds.Distinct();
                }
                else
                {
                    orQueryIds = orQueryIds.Concat(allowedContentIds);
                }
            }

            return andQueries.Count == 0 ? orQueryIds?.Distinct()
                    : orQueryIds == null || orQueries.Count == 0 ? andQueryIds?.Distinct()
                        : andQueryIds?.Where(m => orQueryIds.Contains(m)).Distinct();
        }

        public IQueryable<int>? GetQueryableMetadataByContentId(int contentId, MixContentType? contentType, string metadataType)
        {
            var query = from metadata in _uow.DbContext.MixMetadata
                        join association in _uow.DbContext.MixMetadataContentAssociation
                        on metadata.Id equals association.MetadataId
                        where metadata.MixTenantId == CurrentTenant.Id
                            && (string.IsNullOrEmpty(metadataType) || metadata.Type == metadataType)
                            && association.ContentId == contentId
                            && (!contentType.HasValue || association.ContentType == contentType)
                        select association.Id;
            return query;
        }

        #endregion
    }
}
