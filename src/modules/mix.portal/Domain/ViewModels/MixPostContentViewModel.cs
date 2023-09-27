using Microsoft.AspNetCore.Routing;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using RepoDb;
using RepoDb.Enumerations;

namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixPortalPostContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase<MixCmsContext, MixPostContent, int, MixPortalPostContentViewModel>
    {
        #region Constructors

        public MixPortalPostContentViewModel()
        {
        }

        public MixPortalPostContentViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        public MixPortalPostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public new string MixDatabaseName { get; set; }
        public string ClassName { get; set; }
        public string DetailUrl { get; set; }

        public List<MixUrlAliasViewModel> UrlAliases { get; set; }
        public JObject AdditionalData { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await LoadAliasAsync(cancellationToken);
            await base.ExpandView(cancellationToken);
        }

        public override async Task<int> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            MixPostViewModel parent = new()
            {
                DisplayName = Title,
                Description = Excerpt,
                MixTenantId = MixTenantId
            };

            parent.SetUowInfo(UowInfo, CacheService);
            return await parent.SaveAsync(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixPostContent parentEntity, CancellationToken cancellationToken = default)
        {
            await SaveAliasAsync(parentEntity, cancellationToken);
        }

        private async Task SaveAliasAsync(MixPostContent parentEntity, CancellationToken cancellationToken)
        {
            if (UrlAliases != null)
            {
                foreach (var item in UrlAliases)
                {
                    item.SetUowInfo(UowInfo, CacheService);
                    item.MixTenantId = MixTenantId;
                    item.SourceContentId = parentEntity.Id;
                    item.Type = MixUrlAliasType.Post;
                    await item.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Context.MixPagePostAssociation.RemoveRange(Context.MixPagePostAssociation.Where(m => m.ChildId == Id));
            Context.MixModulePostAssociation.RemoveRange(Context.MixModulePostAssociation.Where(m => m.ChildId == Id));

            if (Repository.GetListQuery(m => m.ParentId == ParentId, cancellationToken).Count() == 1)
            {
                var postRepo = MixPostViewModel.GetRepository(UowInfo, CacheService);
                await Repository.DeleteAsync(Id, cancellationToken);
                await postRepo.DeleteAsync(ParentId, cancellationToken);
            }
            else
            {
                await base.DeleteHandlerAsync(cancellationToken);
            }
        }

        public async Task LoadContributorsAsync(MixIdentityService identityService, CancellationToken cancellationToken = default)
        {
            Contributors = await MixContributorViewModel.GetRepository(UowInfo, CacheService).GetAllAsync(m => m.ContentType == MixContentType.Post && m.IntContentId == Id, cancellationToken);
            foreach (var item in Contributors)
            {
                await item.LoadUserDataAsync(identityService);
            }
        }

        private async Task LoadAliasAsync(CancellationToken cancellationToken = default)
        {
            var aliasRepo = MixUrlAliasViewModel.GetRepository(UowInfo, CacheService);
            UrlAliases = await aliasRepo.GetListAsync(m => m.Type == MixUrlAliasType.Post && m.SourceContentId == Id, cancellationToken);
            DetailUrl = UrlAliases.Count > 0 ? UrlAliases[0].Alias : $"/post/{Id}/{SeoName}";
        }

        public async Task LoadAdditionalDataAsync(
                MixRepoDbRepository mixRepoDbRepository,
                MixCacheService cacheService)
        {
            bool isChanged = false;
            if (AdditionalData == null && !string.IsNullOrEmpty(MixDatabaseName))
            {
                isChanged = true;
                var relationships = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == MixDatabaseName).ToList();
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Post, Id);
                if (obj != null)
                {
                    AdditionalData = ReflectionHelper.ParseObject(obj);

                    foreach (var item in relationships)
                    {

                        mixRepoDbRepository.InitTableName(item.DestinateDatabaseName);
                        var allowsIds = Context.MixDatabaseAssociation
                                .Where(m => m.ParentDatabaseName == MixDatabaseName
                                            && m.ParentId == AdditionalData.Value<int>("id")
                                            && m.ChildDatabaseName == item.DestinateDatabaseName)
                                .Select(m => m.ChildId).ToList();
                        var queries = new List<QueryField>()
                    {
                        new QueryField("Id", Operation.In, allowsIds)
                    };
                        var data = await mixRepoDbRepository.GetListByAsync(queries);
                        var arr = new JArray();
                        if (data != null)
                        {
                            foreach (var dataItem in data)
                            {
                                arr.Add(ReflectionHelper.ParseObject(dataItem));
                            }
                        }
                        AdditionalData.Add(new JProperty(item.DisplayName, JArray.FromObject(arr)));
                    }
                }
            }

            if (isChanged)
            {
                await cacheService.SetAsync($"{Id}/{GetType().FullName}", this, typeof(MixPostContent).FullName, "full");
            }

        }
        #endregion
    }
}
