using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Lib.ViewModels;
using System;
using Mix.Heart.Exceptions;
using System.Threading.Tasks;
using Mix.Lib.Dtos;
using System.Linq;
using Mix.Shared.Enums;
using System.Linq.Expressions;
using Mix.Heart.Extensions;

namespace Mix.Lib.Services
{
    public class MixThemeExportService
    {
        private readonly MixCmsContext _context;
        private readonly UnitOfWorkInfo _uow;
        private SiteDataViewModel _siteData;
        private ExportThemeDto _dto;

        public MixThemeExportService(MixCmsContext context)
        {
            _context = context;
            _uow = new UnitOfWorkInfo(_context);
        }

        #region Export

        public async Task<SiteDataViewModel> ExportSelectedItemsAsync(ExportThemeDto request)
        {
            try
            {
                _dto = request;
                _siteData = new(_dto);

                ExportPostData();

                ExportPageData();

                ExportModuleData();

                ExportAdditionalData();

                ExportDatabaseData();

                ExportUrlAlias();

                ExportConfigurationData();

                ExportLanguageData();

                return _siteData;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        
        #region Export Page Data

        private void ExportPageData()
        {
            ExportPageContents();
            ExportPageModules();
            ExportPagePosts();
        }

        private void ExportPageContents()
        {
            _siteData.PageContents = _context.MixPageContent
                .Where(m => _siteData.PageIds.Contains(m.ParentId))
                .ToList();
            _siteData.PageContentIds = _siteData.PageContents.Select(p => p.Id).ToList();
        }

        private void ExportPageModules()
        {
            var pageModules = _context.MixPageModuleAssociation.Where(
                    m => _siteData.PageIds.Any(p => p == m.LeftId));

            _siteData.PageModules.AddRange(pageModules);

            // Get Modules unchecked when export but needed in selected pages.
            var moduleIds = pageModules
                .Where(m => _siteData.ModuleIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();

            var modules = _context.MixModule
                .Where(m =>
                    moduleIds.Any(p => p == m.Id));

            // Add to selected List
            _siteData.ModuleIds.AddRange(moduleIds);
            _siteData.Modules.AddRange(modules);
        }

        private void ExportPagePosts()
        {
            var pagePosts = _context.MixPagePostAssociation.Where(
                    m => _siteData.PageIds.Any(p => p == m.LeftId));

            _siteData.PagePosts.AddRange(pagePosts);

            // Get Posts unchecked when export but needed in selected pages.
            var postIds = pagePosts
                .Where(m => _siteData.PostIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();

            var posts = _context.MixPost
                .Where(m =>
                    postIds.Any(p => p == m.Id));

            // Add to selected List
            _siteData.PostIds.AddRange(postIds);
            _siteData.Posts.AddRange(posts);
        }

        #endregion Export Page

        #region Export Module Data

        private void ExportModuleData()
        {
            ExportModuleContents();
            ExportModuleDatas();
            ExportModulePosts();
        }

        private void ExportModuleContents()
        {
            _siteData.ModuleContents = _context.MixModuleContent
                .Where(m => _siteData.ModuleIds.Contains(m.ParentId))
                .ToList();
            _siteData.ModuleContentIds = _siteData.ModuleContents.Select(p => p.Id).ToList();
        }

        private void ExportModuleDatas()
        {
            var data = _context.MixModuleData.Where(
                    m => _dto.ExportData.ModuleIds.Any(p => p == m.ParentId));
            _siteData.ModuleDatas.AddRange(data);
        }

        private void ExportModulePosts()
        {
            var modulePosts = _context.MixModulePostAssociation.Where(
                    m => _siteData.ModuleIds.Any(p => p == m.LeftId));

            _siteData.ModulePosts.AddRange(modulePosts);

            // Get Posts unchecked when export but needed in selected modules.
            var postIds = modulePosts
                .Where(m => _siteData.PostIds.Any(n => m.RightId == n))
                .Select(m => m.RightId).ToList();

            var posts = _context.MixPost
                .Where(m =>
                    postIds.Any(p => p == m.Id));

            // Add to selected List
            _siteData.PostIds.AddRange(postIds);
            _siteData.Posts.AddRange(posts);
        }
        #endregion Export Module

        #region Export Database Data

        private void ExportDatabaseData()
        {
            ExportColumns();
            ExportDataContents();
            ExportDataAssociations();
            ExportValues();
            ExportDatas();
        }

        private void ExportColumns()
        {
            _siteData.DatabaseColumns = _context.MixDatabaseColumn.Where(
                    m => _siteData.DatabaseIds.Contains(m.MixDatabaseId)).ToList();
        }

        private void ExportValues()
        {
            _siteData.DataContentValues = _context.MixDataContentValue.Where(
                            m => _siteData.DataContentIds.Distinct().Any(n => n == m.ParentId)).ToList();
        }

        private void ExportDatas()
        {
            var data = _context.MixDataContent.Where(
                        m => _siteData.DataContentIds.Any(p => p == m.Id))
                        .Select(m => m.ParentId);
            _siteData.Datas = _context.MixData.Where(m => data.Any(n => n == m.Id)).ToList();
        }

        private void ExportDataContents()
        {
            var data = _context.MixDataContent.Where(
                   m => _dto.ExportData.DatabaseIds.Any(p => p == m.MixDatabaseId));
            _siteData.DataContentIds.AddRange(data.Select(m => m.Id));
            _siteData.DataContents = _context.MixDataContent
                .Where(m => _siteData.DataContentIds.Distinct().Any(n => n == m.Id)).ToList();
        }

        private void ExportDataAssociations()
        {
            var data = _context.MixDataContentAssociation.Where(
                   m => _dto.ExportData.DatabaseIds.Any(p => p == m.MixDatabaseId));
            _siteData.DataContentAssociationIds.AddRange(data.Select(m => m.Id));
            _siteData.DataContentAssociations = _context.MixDataContentAssociation
                .Where(m => _siteData.DataContentAssociationIds.Distinct().Any(n => n == m.Id)).ToList();
        }
        #endregion Export Module

        private void ExportPostData()
        {
            _siteData.PostContents = _context.MixPostContent
                .Where(m => _siteData.PostIds.Contains(m.ParentId))
                .ToList();
            _siteData.PostContentIds = _siteData.PostContents.Select(p => p.Id).ToList();
        }
        
        private void ExportConfigurationData()
        {
            _siteData.ConfigurationContents = _context.MixConfigurationContent
                .Where(m => _siteData.ConfigurationIds.Contains(m.ParentId))
                .ToList();
        }
        
        private void ExportLanguageData()
        {
            _siteData.LanguageContents = _context.MixLanguageContent
                .Where(m => _siteData.LanguageIds.Contains(m.ParentId))
                .ToList();
        }

        private void ExportUrlAlias()
        {
            Expression<Func<MixUrlAlias, bool>> predicate =
                m => m.Type == MixUrlAliasType.Page
                    && m.SourceContentId.HasValue
                    && _siteData.PageContentIds.Contains(m.SourceContentId.Value);
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Post
                    && m.SourceContentId.HasValue
                    && _siteData.PostContentIds.Contains(m.SourceContentId.Value));
            predicate = predicate.Or(m => m.Type == MixUrlAliasType.Module
                    && m.SourceContentId.HasValue
                    && _siteData.ModuleContentIds.Contains(m.SourceContentId.Value));

            _siteData.MixUrlAliases = _context.MixUrlAlias.Where(predicate).ToList();
        }

        private void ExportAdditionalData()
        {
            Expression<Func<MixDataContentAssociation, bool>> predicate =
                m => m.IntParentId.HasValue &&  
                    (
                        (_siteData.PageIds.Any(p => p == m.IntParentId.Value) 
                        && m.ParentType == MixDatabaseParentType.Page)
                        || (_siteData.PostIds.Any(p => p == m.IntParentId.Value) 
                            && m.ParentType == MixDatabaseParentType.Post)
                        || (_siteData.ModuleIds.Any(p => p == m.IntParentId.Value) 
                            && m.ParentType == MixDatabaseParentType.Module));
            var associations = _context.MixDataContentAssociation
                    .Where(predicate);
            _siteData.DataContentIds.AddRange(associations.Select(m => m.DataContentId));
        }

        #endregion Export
    }
}
