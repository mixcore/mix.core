using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Database.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Mix.Heart.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Mix.Portal.Domain.ViewModels;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// MCP Tool for page-module association operations
    /// </summary>
    [McpServerToolType]
    public class MixPageModuleTool : BaseMcpTool
    {
        public MixPageModuleTool(
            AppSettingsService appSettingsService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ILogger<MixPageModuleTool> logger)
            : base(appSettingsService, cmsUow, logger)
        {
        }

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

                var viewModel = await MixPageModuleViewModel.GetRepository(_cmsUow, null)
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

                var repo = MixPageModuleViewModel.GetRepository(_cmsUow, null);
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

        /// <summary>
        /// Update association priorities in bulk
        /// </summary>
        [McpServerTool, Description("Update page-module association priorities in bulk")]
        public async Task<string> UpdatePageModulePriorities(
            [Description("JSON array of {Id, Priority} objects")] string prioritiesJson,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(prioritiesJson)) throw new McpException("Priorities JSON cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating page-module priorities");

                var priorities = System.Text.Json.JsonSerializer.Deserialize<List<PriorityUpdate>>(prioritiesJson);
                if (priorities == null || priorities.Count == 0)
                    throw new McpException("Invalid priorities JSON format.");

                var updatedCount = 0;

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

                await _cmsUow.DbContext.SaveChangesAsync(ct);

                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Updated {updatedCount} page-module priorities",
                    UpdatedCount = updatedCount
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdatePageModulePriorities");
        }

        #region Helper Classes

        private class PriorityUpdate
        {
            public int Id { get; set; }
            public int Priority { get; set; }
        }

        #endregion
    }
}