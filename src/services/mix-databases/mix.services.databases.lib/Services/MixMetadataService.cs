using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.ViewModels;
using System.Linq.Expressions;
using Mix.Lib.ViewModels;
using Mix.Lib.Models.Common;
using Mix.Heart.Models;
using Org.BouncyCastle.Asn1.Cms;
using Mix.Heart.ViewModel;
using Mix.Heart.Repository;
using Mix.Heart.Entities;
using Mix.Shared.Models;
using Mix.Services.Databases.Lib.Models;
using Mix.Heart.Extensions;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixMetadataService : TenantServiceBase
    {
        private readonly MixIdentityService _identityService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly UnitOfWorkInfo<MixServiceDatabaseDbContext> _uow;

        public MixMetadataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow,
            MixIdentityService identityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _identityService = identityService;
            _cmsUow = cmsUow;
        }


        public async Task<List<PostMetadata>> GetMetadataAsync(string[]? includes = null, string[]? excepts = null)
        {
            Expression<Func<MixMetadata, bool>> predicate = m => m.MixTenantId == CurrentTenant.Id;
            predicate = predicate.AndAlsoIf(includes != null, m => includes!.Contains(m.Type));
            predicate = predicate.AndAlsoIf(excepts != null, m => !excepts!.Contains(m.Type));
            var data = await MixMetadataViewModel.GetRepository(_uow).GetAllAsync(predicate);
            return data.GroupBy(m => m.Type)
                    .Select(m => new PostMetadata()
                    {
                        MetadataType = m.Key!,
                        Data = m.ToList()
                    }).ToList();
        }

        public async Task<PagingResponseModel<TView>?> GetPostByMetadataAsync<TView, TEntity>(
                string[] metadataSeoContents,
                MetadataParentType contentType,
                HttpRequest request)
            where TView : ViewModelBase<MixCmsContext, MixPostContent, int, TView>
             where TEntity : EntityBase<int>
        {
            SearchQueryModel<TEntity, int> searchRequest = new(request);
            var query = GetQueryableContentIdByMetadataSeoContent(metadataSeoContents, contentType);
            if (query != null)
            {
                // TODO: Share dbcontext transaction
                var allowIds = await query.ToListAsync();
                var repo = new Repository<MixCmsContext, MixPostContent, int, TView>(_cmsUow);
                return await repo.GetPagingAsync(m => m.MixTenantId == CurrentTenant.Id
                                && allowIds.Any(n => n == m.Id),
                                searchRequest.PagingData);
            }
            return default;
        }

        public async Task<MixMetadataViewModel> GetOrCreateMetadata(CreateMetadataDto dto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var metadata = await MixMetadataViewModel.GetRepository(_uow).GetSingleAsync(m => m.Content == dto.Content && m.Type == dto.Type, cancellationToken);
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
            ReflectionHelper.MapObject(dto, metadata);
            await metadata.SaveAsync(cancellationToken);
            return metadata;
        }

        public async Task CreateMetadataContentAssociation(CreateMetadataContentAssociationDto dto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            MixMixMetadataContentAsscociationViewModel association = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.MapObject(dto, association);
            await association.SaveAsync(cancellationToken);
        }

        public async Task<PagingResponseModel<MixMixMetadataContentAsscociationViewModel>> GetMetadataByContentId(
            int intContentId,
            MetadataParentType? contentType,
            string metadataType, PagingRequestModel pagingData)
        {
            var query = GetQueryableMetadataByContentId(intContentId, contentType, metadataType);
            if (query != null)
            {
                return await MixMixMetadataContentAsscociationViewModel.GetRepository(_uow)
                            .GetPagingAsync(
                                m => m.MixTenantId == CurrentTenant.Id
                                        && query.Any(n => n == m.Id),
                                        pagingData);
            }
            return default;
        }

        public async Task DeleteMetadataContentAssociation(int id, CancellationToken cancellationToken = default)
        {
            var association = await MixMixMetadataContentAsscociationViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == id);
            if (association != null)
            {
                await association.DeleteAsync(cancellationToken);
            }
        }

        #region IQueryables

        public IQueryable<int>? GetQueryableContentIdByMetadataSeoContent(string[] metadataSeoContents, MetadataParentType contentType)
        {
            var query = from metadata in _uow.DbContext.MixMetadata
                        join association in _uow.DbContext.MixMetadataContentAssociation
                        on metadata.Id equals association.MetadataId
                        where metadata.MixTenantId == CurrentTenant.Id
                            && metadataSeoContents.Contains(metadata.SeoContent)
                            && association.ContentType == contentType
                        select association.ContentId;
            return query;
        }

        public IQueryable<int>? GetQueryableMetadataByContentId(int contentId, MetadataParentType? contentType, string metadataType)
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
