using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Database.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Mix.Constant.Enums;
using Mix.Heart.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mix.Portal.Domain.ViewModels;
using Mix.Lib.ViewModels;
using Mix.Heart.Services;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// MCP Tool for navigation models (page-module, page-post, module-post associations)
    /// </summary>
    [McpServerToolType]
    public class MixNavigationTool : BaseMcpTool
    {
        public MixNavigationTool(
            AppSettingsService appSettingsService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ILogger<MixNavigationTool> logger,
            MixCacheService cacheService)
            : base(appSettingsService, cmsUow, logger, cacheService)
        {
        }

        #region Page-Module Association Operations

        /// <summary>
        /// Create a new page-module association
        /// </summary>
        [McpServerTool, Description("Create a new page-module association")]
        public async Task<string> CreatePageModuleAssociation(
            [Description("Page content ID")] int pageContentId,
            [Description("Module content ID")] int moduleContentId,
            [Description("Display priority (lower values appear first)")] int priority = 0,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (pageContentId <= 0) throw new McpException("Page content ID must be greater than 0.");
            if (moduleContentId <= 0) throw new McpException("Module content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating page-module association: Page {PageId}, Module {ModuleId}", pageContentId, moduleContentId);

                // Check if association already exists
                var exists = await _cmsUow.DbContext.MixPageModuleAssociation
                    .AnyAsync(m => m.ParentId == pageContentId && m.ChildId == moduleContentId && m.TenantId == tenantId, ct);
                if (exists)
                    throw new McpException($"Association between page {pageContentId} and module {moduleContentId} already exists.");

                var viewModel = new MixPageModuleViewModel(_cmsUow)
                {
                    ParentId = pageContentId,
                    ChildId = moduleContentId,
                    Priority = priority,
                    TenantId = tenantId
                };

                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page-module association created successfully",
                    Data = viewModel
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreatePageModuleAssociation");
        }

        /// <summary>
        /// List modules for a page
        /// </summary>
        [McpServerTool, Description("List modules for a page")]
        public async Task<string> ListModulesForPage(
            [Description("Page content ID")] int pageContentId,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (pageContentId <= 0) throw new McpException("Page content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing modules for page {PageId}", pageContentId);

                var associations = await _cmsUow.DbContext.MixPageModuleAssociation
                    .Where(m => m.ParentId == pageContentId && m.TenantId == tenantId)
                    .OrderBy(m => m.Priority)
                    .ToListAsync(ct);

                var viewModels = associations.Select(a => new MixPageModuleViewModel(a, _cmsUow)).ToList();
                foreach (var vm in viewModels)
                {
                    await vm.ExpandView(ct);
                }

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = viewModels,
                    TotalCount = viewModels.Count
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListModulesForPage");
        }

        /// <summary>
        /// Update page-module association priority
        /// </summary>
        [McpServerTool, Description("Update page-module association priority")]
        public async Task<string> UpdatePageModuleAssociation(
            [Description("Association ID")] int associationId,
            [Description("New priority")] int? priority = null,
            CancellationToken cancellationToken = default)
        {
            if (associationId <= 0) throw new McpException("Association ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating page-module association {AssociationId}", associationId);

                var viewModel = await MixPageModuleViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.Id == associationId, ct);
                if (viewModel == null)
                    throw new McpException($"Page-module association with ID {associationId} not found.");

                if (priority.HasValue) viewModel.Priority = priority.Value;

                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page-module association updated successfully",
                    Data = viewModel
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdatePageModuleAssociation");
        }

        /// <summary>
        /// Delete page-module association
        /// </summary>
        [McpServerTool, Description("Delete page-module association")]
        public async Task<string> DeletePageModuleAssociation(
            [Description("Association ID")] int associationId,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (associationId <= 0) throw new McpException("Association ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete association, confirmDelete must be 'YES'.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting page-module association {AssociationId}", associationId);

                var repo = MixPageModuleViewModel.GetRepository(_cmsUow, _cacheService);
                var exists = await repo.GetFirstAsync(m => m.Id == associationId, ct);
                if (exists == null)
                    throw new McpException($"Page-module association with ID {associationId} not found.");

                await repo.DeleteAsync(associationId, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page-module association deleted successfully",
                    Id = associationId
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeletePageModuleAssociation");
        }

        #endregion

        #region Page-Post Association Operations

        /// <summary>
        /// Create a new page-post association
        /// </summary>
        [McpServerTool, Description("Create a new page-post association")]
        public async Task<string> CreatePagePostAssociation(
            [Description("Page content ID")] int pageContentId,
            [Description("Post content ID")] int postContentId,
            [Description("Display priority (lower values appear first)")] int priority = 0,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (pageContentId <= 0) throw new McpException("Page content ID must be greater than 0.");
            if (postContentId <= 0) throw new McpException("Post content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating page-post association: Page {PageId}, Post {PostId}", pageContentId, postContentId);

                // Check if association already exists
                var exists = await _cmsUow.DbContext.MixPagePostAssociation
                    .AnyAsync(m => m.ParentId == pageContentId && m.ChildId == postContentId && m.TenantId == tenantId, ct);
                if (exists)
                    throw new McpException($"Association between page {pageContentId} and post {postContentId} already exists.");

                var viewModel = new MixPagePostAssociationViewModel(_cmsUow)
                {
                    ParentId = pageContentId,
                    ChildId = postContentId,
                    Priority = priority,
                    TenantId = tenantId
                };

                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page-post association created successfully",
                    Data = viewModel
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreatePagePostAssociation");
        }

        /// <summary>
        /// List posts for a page
        /// </summary>
        [McpServerTool, Description("List posts for a page")]
        public async Task<string> ListPostsForPage(
            [Description("Page content ID")] int pageContentId,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (pageContentId <= 0) throw new McpException("Page content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing posts for page {PageId}", pageContentId);

                var associations = await _cmsUow.DbContext.MixPagePostAssociation
                    .Where(m => m.ParentId == pageContentId && m.TenantId == tenantId)
                    .OrderBy(m => m.Priority)
                    .ToListAsync(ct);

                var viewModels = associations.Select(a => new MixPagePostAssociationViewModel(a, _cmsUow)).ToList();
                foreach (var vm in viewModels)
                {
                    await vm.ExpandView(ct);
                }

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = viewModels,
                    TotalCount = viewModels.Count
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListPostsForPage");
        }

        /// <summary>
        /// Delete page-post association
        /// </summary>
        [McpServerTool, Description("Delete page-post association")]
        public async Task<string> DeletePagePostAssociation(
            [Description("Association ID")] int associationId,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (associationId <= 0) throw new McpException("Association ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete association, confirmDelete must be 'YES'.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting page-post association {AssociationId}", associationId);

                var repo = MixPagePostAssociationViewModel.GetRepository(_cmsUow, _cacheService);
                var exists = await repo.GetFirstAsync(m => m.Id == associationId, ct);
                if (exists == null)
                    throw new McpException($"Page-post association with ID {associationId} not found.");

                await repo.DeleteAsync(associationId, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page-post association deleted successfully",
                    Id = associationId
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeletePagePostAssociation");
        }

        #endregion

        #region Module-Post Association Operations

        /// <summary>
        /// Create a new module-post association
        /// </summary>
        [McpServerTool, Description("Create a new module-post association")]
        public async Task<string> CreateModulePostAssociation(
            [Description("Module content ID")] int moduleContentId,
            [Description("Post content ID")] int postContentId,
            [Description("Display priority (lower values appear first)")] int priority = 0,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (moduleContentId <= 0) throw new McpException("Module content ID must be greater than 0.");
            if (postContentId <= 0) throw new McpException("Post content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating module-post association: Module {ModuleId}, Post {PostId}", moduleContentId, postContentId);

                // Check if association already exists
                var exists = await _cmsUow.DbContext.MixModulePostAssociation
                    .AnyAsync(m => m.ParentId == moduleContentId && m.ChildId == postContentId && m.TenantId == tenantId, ct);
                if (exists)
                    throw new McpException($"Association between module {moduleContentId} and post {postContentId} already exists.");

                var viewModel = new MixModulePostAssociationViewModel(_cmsUow)
                {
                    ParentId = moduleContentId,
                    ChildId = postContentId,
                    Priority = priority,
                    TenantId = tenantId
                };

                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Module-post association created successfully",
                    Data = viewModel
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateModulePostAssociation");
        }

        /// <summary>
        /// List posts for a module
        /// </summary>
        [McpServerTool, Description("List posts for a module")]
        public async Task<string> ListPostsForModule(
            [Description("Module content ID")] int moduleContentId,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (moduleContentId <= 0) throw new McpException("Module content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing posts for module {ModuleId}", moduleContentId);

                var associations = await _cmsUow.DbContext.MixModulePostAssociation
                    .Where(m => m.ParentId == moduleContentId && m.TenantId == tenantId)
                    .OrderBy(m => m.Priority)
                    .ToListAsync(ct);

                var viewModels = associations.Select(a => new MixModulePostAssociationViewModel(a, _cmsUow)).ToList();
                foreach (var vm in viewModels)
                {
                    await vm.ExpandView(ct);
                }

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = viewModels,
                    TotalCount = viewModels.Count
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListPostsForModule");
        }

        /// <summary>
        /// Delete module-post association
        /// </summary>
        [McpServerTool, Description("Delete module-post association")]
        public async Task<string> DeleteModulePostAssociation(
            [Description("Association ID")] int associationId,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (associationId <= 0) throw new McpException("Association ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete association, confirmDelete must be 'YES'.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting module-post association {AssociationId}", associationId);

                var repo = MixModulePostAssociationViewModel.GetRepository(_cmsUow, _cacheService);
                var exists = await repo.GetFirstAsync(m => m.Id == associationId, ct);
                if (exists == null)
                    throw new McpException($"Module-post association with ID {associationId} not found.");

                await repo.DeleteAsync(associationId, ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Module-post association deleted successfully",
                    Id = associationId
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeleteModulePostAssociation");
        }

        #endregion

        #region Navigation Tree Operations

        /// <summary>
        /// Get navigation tree for a page (includes modules and their posts)
        /// </summary>
        [McpServerTool, Description("Get complete navigation tree for a page (includes modules and their posts)")]
        public async Task<string> GetPageNavigationTree(
            [Description("Page content ID")] int pageContentId,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (pageContentId <= 0) throw new McpException("Page content ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Building navigation tree for page {PageId}", pageContentId);

                // Get page modules
                var pageModules = await _cmsUow.DbContext.MixPageModuleAssociation
                    .Where(m => m.ParentId == pageContentId && m.TenantId == tenantId)
                    .OrderBy(m => m.Priority)
                    .ToListAsync(ct);

                // Get page posts
                var pagePosts = await _cmsUow.DbContext.MixPagePostAssociation
                    .Where(m => m.ParentId == pageContentId && m.TenantId == tenantId)
                    .OrderBy(m => m.Priority)
                    .ToListAsync(ct);

                // Build navigation tree
                var navigationTree = new
                {
                    PageId = pageContentId,
                    Modules = pageModules.Select(pm => new
                    {
                        ModuleId = pm.ChildId,
                        Priority = pm.Priority,
                        AssociationId = pm.Id
                    }).ToList(),
                    Posts = pagePosts.Select(pp => new
                    {
                        PostId = pp.ChildId,
                        Priority = pp.Priority,
                        AssociationId = pp.Id
                    }).ToList()
                };

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = navigationTree
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetPageNavigationTree");
        }

        /// <summary>
        /// Update association priorities in bulk
        /// </summary>
        [McpServerTool, Description("Update association priorities in bulk")]
        public async Task<string> UpdateAssociationPriorities(
            [Description("Association type (PageModule, PagePost, ModulePost)")] string associationType,
            [Description("JSON array of {Id, Priority} objects")] string prioritiesJson,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(associationType)) throw new McpException("Association type cannot be empty.");
            if (string.IsNullOrWhiteSpace(prioritiesJson)) throw new McpException("Priorities JSON cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating {AssociationType} priorities", associationType);

                var priorities = System.Text.Json.JsonSerializer.Deserialize<List<PriorityUpdate>>(prioritiesJson);
                if (priorities == null || priorities.Count == 0)
                    throw new McpException("Invalid priorities JSON format.");

                var updatedCount = 0;

                switch (associationType.ToLower())
                {
                    case "pagemodule":
                        foreach (var update in priorities)
                        {
                            var entity = await _cmsUow.DbContext.MixPageModuleAssociation
                                .FirstOrDefaultAsync(m => m.Id == update.Id, ct);
                            if (entity != null)
                            {
                                entity.Priority = update.Priority;
                                updatedCount++;
                            }
                        }
                        break;

                    case "pagepost":
                        foreach (var update in priorities)
                        {
                            var entity = await _cmsUow.DbContext.MixPagePostAssociation
                                .FirstOrDefaultAsync(m => m.Id == update.Id, ct);
                            if (entity != null)
                            {
                                entity.Priority = update.Priority;
                                updatedCount++;
                            }
                        }
                        break;

                    case "modulepost":
                        foreach (var update in priorities)
                        {
                            var entity = await _cmsUow.DbContext.MixModulePostAssociation
                                .FirstOrDefaultAsync(m => m.Id == update.Id, ct);
                            if (entity != null)
                            {
                                entity.Priority = update.Priority;
                                updatedCount++;
                            }
                        }
                        break;

                    default:
                        throw new McpException($"Unknown association type: {associationType}");
                }

                await _cmsUow.DbContext.SaveChangesAsync(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Updated {updatedCount} {associationType} priorities",
                    UpdatedCount = updatedCount
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdateAssociationPriorities");
        }

        #endregion

        #region Helper Classes

        private class PriorityUpdate
        {
            public int Id { get; set; }
            public int Priority { get; set; }
        }

        #endregion
    }
}
