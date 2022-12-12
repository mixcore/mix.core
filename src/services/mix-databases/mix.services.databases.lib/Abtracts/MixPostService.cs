using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Models;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Databases.Lib.Abtracts
{
    public abstract class MixPostServiceBase<TView> : TenantServiceBase
        where TView : ViewModelBase<MixCmsContext, MixPostContent, int, TView>
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly MixMetadataService _metadataService;

        protected MixPostServiceBase(UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor)
        {
            _uow = uow;
            _metadataService = metadataService;
        }

        public async Task<List<TView>> GetRelatedPosts(int postId, CancellationToken cancellationToken = default)
        {
            using var postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(_uow);
            var relatedIds = from links in _uow.DbContext.MixPostPostAssociation
                             where links.ParentId == postId && links.MixTenantId == CurrentTenant.Id
                             select links.ChildId;
            var result = await postRepo.GetListAsync(m => relatedIds.Contains(m.Id), cancellationToken);
            return result;
        }

        public async Task<PagingResponseModel<TView>> Search(HttpRequest httpRequest)
        {
            return await SearchPosts(new(httpRequest));
        }

        public async Task<PagingResponseModel<TView>> SearchPosts(SearchPostQueryModel searchRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var postRepo = new Repository<MixCmsContext, MixPostContent, int, TView>(_uow);
                searchRequest.Predicate = searchRequest.Predicate.AndAlso(ApplyMetadataQueries(searchRequest));
                var result = await postRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        private Expression<Func<MixPostContent, bool>> ApplyMetadataQueries(SearchPostQueryModel searchRequest)
        {
            Expression<Func<MixPostContent, bool>> predicate = m => true;
            if (searchRequest.MetadataQueries.Count == 0)
            {
                return predicate;
            }

            IQueryable<int> allowedIdQuery = default;
            foreach (var item in searchRequest.MetadataQueries)
            {
                var query = _metadataService.GetQueryableContentIdByMetadataSeoContent(item.Value, MetadataParentType.Post);
                allowedIdQuery = allowedIdQuery == default ?
                    query
                    : allowedIdQuery.Union(query);
            }
            var allowedIds = allowedIdQuery.ToList();
            predicate = predicate.AndAlsoIf(allowedIdQuery != null, m => allowedIds.Contains(m.Id));
            return predicate;
        }
    }
}
