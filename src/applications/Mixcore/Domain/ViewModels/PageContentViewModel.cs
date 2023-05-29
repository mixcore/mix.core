﻿using Microsoft.EntityFrameworkCore;
using Mix.Heart.Helpers;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Models;
using System.Reflection;

namespace Mixcore.Domain.ViewModels
{
    public sealed class PageContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, PageContentViewModel>
    {
        #region Constructors

        public PageContentViewModel()
        {
        }

        public PageContentViewModel(MixPageContent entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }
        public string ClassName { get; set; }
        public string DetailUrl => $"/page/{Id}/{SeoName}";
        public Guid? AdditionalDataId { get; set; }

        public List<ModuleContentViewModel> Modules { get; set; }
        public PagingResponseModel<PagePostAssociationViewModel> Posts { get; set; }
        public JObject AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method

        public async Task LoadDataAsync(
            MixRepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService,
            PagingRequestModel pagingModel, MixCacheService cacheService)
        {
            await LoadAdditionalDataAsync(mixRepoDbRepository);
            await LoadModulesAsync(mixRepoDbRepository, metadataService, cacheService);
            await LoadPostsAsync(pagingModel, mixRepoDbRepository, metadataService, cacheService);
        }


        public ModuleContentViewModel GetModule(string moduleName)
        {
            return Modules?.FirstOrDefault(m => m.SystemName == moduleName);
        }
        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        #endregion

        #region Private Methods
        private async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            if (!string.IsNullOrEmpty(MixDatabaseName))
            {
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Page, Id);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
            }
        }
        private async Task LoadModulesAsync(MixRepoDbRepository mixRepoDbRepository, IMixMetadataService metadataService, MixCacheService cacheService)
        {
            var moduleIds = await Context.MixPageModuleAssociation
                    .AsNoTracking()
                    .Where(p => p.ParentId == Id)
                    .OrderBy(m => m.Priority)
                    .Select(m => m.ChildId)
                    .ToListAsync();

            var paging = new PagingModel();
            Modules = new List<ModuleContentViewModel>();
            var moduleRepo = ModuleContentViewModel.GetRepository(UowInfo, CacheService);
            foreach (var moduleId in moduleIds)
            {
                var item = await moduleRepo.GetSingleAsync(m => m.Id == moduleId);
                await item.LoadData(paging, mixRepoDbRepository, metadataService, cacheService);

                Modules.Add(item);
            }
        }
        private async Task LoadPostsAsync(PagingRequestModel pagingModel, MixRepoDbRepository mixRepoDbRepository, IMixMetadataService metadataService, MixCacheService cacheService)
        {
            Posts = await PagePostAssociationViewModel.GetRepository(UowInfo, CacheService).GetPagingAsync(m => m.ParentId == Id, pagingModel);
            foreach (var item in Posts.Items)
            {
                item.SetUowInfo(UowInfo, CacheService);
                await item.LoadPost(mixRepoDbRepository, metadataService, cacheService);
            }
        }

        #endregion
    }
}
