using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixMetadataService : TenantServiceBase
    {
        private readonly MixIdentityService _identityService;
        private MixServiceDatabaseDbContext _permissionDbContext;
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private UnitOfWorkInfo<MixServiceDatabaseDbContext> _uow;

        public MixMetadataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow,
            MixIdentityService identityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _permissionDbContext = _uow.DbContext;
            _identityService = identityService;
            _cmsUOW = cmsUOW;
        }

        public async Task<List<MixMetadataViewModel>> GetPermissionAsyncs(Guid userId)
        {
            var permissions = _permissionDbContext.UserPermission.Where(m => m.MixTenantId == CurrentTenant.Id && m.UserId == userId);
            Expression<Func<MixMetadata, bool>> predicate =
                m => m.MixTenantId == CurrentTenant.Id
                && permissions.Any(p => p.PermissionId == m.Id);
            var result = await MixMetadataViewModel.GetRepository(_uow).GetAllAsync(predicate);
            return result;
        }

        public IQueryable<int>? GetQueryableContentIdByMetadataSeoContent(string metadataSeoContent, MetadataParentType contentType)
        {
            var query = from metadata in _uow.DbContext.MixMetadata
                        join association in _uow.DbContext.MixMetadataContentAssociation
                        on metadata.Id equals association.MetadataId
                        where metadata.MixTenantId == CurrentTenant.Id
                            && metadata.SeoContent == metadataSeoContent
                            && association.ContentType == contentType
                        select association.Id;
            return query;
        }

        public async Task<dynamic?> GetQueryableContentByMetadataSeoContentAsync(string metadataSeoContent, MetadataParentType contentType, Mix.Lib.Models.Common.SearchQueryModel<MixMetadata, int> searchRequest)
        {
            var query = GetQueryableContentIdByMetadataSeoContent(metadataSeoContent, contentType);
            if (query != null)
            {
                // TODO: Share dbcontext transaction
                var allowIds = await query.ToListAsync();
                switch (contentType)
                {
                    case MetadataParentType.MixDatabse:
                        break;
                    case MetadataParentType.Post:
                        var result = await MixPostContentViewModel.GetRepository(_cmsUOW)
                            .GetPagingAsync(m => m.MixTenantId == CurrentTenant.Id
                                && allowIds.Any(n => n == m.Id),
                                searchRequest.PagingData);
                        return result;
                    case MetadataParentType.Page:
                        break;
                    case MetadataParentType.Module:
                        break;
                    default:
                        break;
                }
            }
            return default;
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
    }
}
