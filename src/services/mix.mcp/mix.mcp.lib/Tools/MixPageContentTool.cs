using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.UnitOfWork;
using Mix.MCP.Lib.Models;
using Mix.Lib.ViewModels;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Mix.Constant.Enums;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Heart.Services;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// MCP Tool for CRUD operations on Mix Page Content
    /// </summary>
    [McpServerToolType]
    public class MixPageContentTool : BaseMcpTool
    {

        /// <summary>
        /// Initializes a new instance of the MixPageContentTool class
        /// </summary>
        public MixPageContentTool(
            AppSettingsService appSettingsService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ILogger<MixPageContentTool> logger,
            MixCacheService cacheService)
            : base(appSettingsService, cmsUow, logger, cacheService)
        {
        }

        /// <summary>
        /// Create a new page content
        /// </summary>
        [McpServerTool, Description("Create a new page content with specified properties")]
        public async Task<string> CreatePageContent(
            [Description("Page title")] string title,
            [Description("Page content body")] string content,
            [Description("SEO name for the page")] string seoName,
            [Description("Template ID")] int templateId,
            [Description("Layout ID")] int layoutId = 1,
            [Description("Page excerpt/description")] string? excerpt = null,
            [Description("Page type (Home, Article, etc.)")] string pageType = "Home",
            [Description("Page size for pagination")] int? pageSize = null,
            [Description("Culture code (e.g., 'en-us')")] string? culture = null,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new McpException("Title cannot be empty.");
            if (templateId == 0)
                throw new McpException("Template Id cannot be empty.");
            if (string.IsNullOrWhiteSpace(content))
                throw new McpException("Content cannot be empty.");
            if (string.IsNullOrWhiteSpace(seoName))
                throw new McpException("SEO name cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating page content with title: {Title}, SEO name: {SeoName}", title, seoName);

                // Check if SEO name already exists
                var existing = await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.SeoName == seoName && m.TenantId == tenantId, ct);
                if (existing != null)
                    throw new McpException($"A page with SEO name '{seoName}' already exists.");

                MixPageType typeEnum = MixPageType.Home;
                Enum.TryParse(pageType, true, out typeEnum);

                var viewModel = new MixPageContentViewModel(_cmsUow)
                {
                    Title = title,
                    Content = content,
                    SeoName = seoName,
                    TemplateId = templateId,
                    LayoutId = layoutId,
                    Excerpt = excerpt ?? string.Empty,
                    TenantId = tenantId,
                    PageSize = pageSize,
                    Type = typeEnum,
                    Specificulture = culture ?? _appSettingsService.AppSettings.DefaultCulture,
                    Status = MixContentStatus.Published,
                    CreatedDateTime = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };
                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);
                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Message = $"Page content '{title}' created successfully",
                    Data = viewModel
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreatePageContent");
        }

        /// <summary>
        /// Get page content by ID
        /// </summary>
        [McpServerTool, Description("Retrieve page content by its ID")]
        public async Task<string> GetPageContent(
            [Description("Page content ID")] int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new McpException("ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Retrieving page content with ID: {Id}", id);
                var viewModel = await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.Id == id, ct);
                if (viewModel == null)
                    throw new McpException($"Page content with ID {id} not found.");
                await viewModel.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Data = viewModel }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetPageContent");
        }

        /// <summary>
        /// Get page content by SEO name
        /// </summary>
        [McpServerTool, Description("Retrieve page content by its SEO name")]
        public async Task<string> GetPageContentBySeoName(
            [Description("SEO name of the page")] string seoName,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(seoName))
                throw new McpException("SEO name cannot be empty.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Retrieving page content with SEO name: {SeoName}", seoName);
                var viewModel = await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.SeoName == seoName && m.TenantId == tenantId, ct);
                if (viewModel == null)
                    throw new McpException($"Page content with SEO name '{seoName}' not found.");
                await viewModel.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Data = viewModel }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetPageContentBySeoName");
        }

        /// <summary>
        /// Update existing page content
        /// </summary>
        [McpServerTool, Description("Update an existing page content")]
        public async Task<string> UpdatePageContent(
            [Description("Page content ID to update")] int id,
            [Description("New page title")] string? title = null,
            [Description("New page content body")] string? content = null,
            [Description("New SEO name")] string? seoName = null,
            [Description("New page excerpt/description")] string? excerpt = null,
            [Description("New content status (0=Preview, 1=Published, 2=Draft)")] int? status = null,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new McpException("ID must be greater than 0.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating page content with ID: {Id}", id);
                var viewModel = await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.Id == id, ct);
                if (viewModel == null)
                    throw new McpException($"Page content with ID {id} not found.");
                if (!string.IsNullOrWhiteSpace(seoName) && seoName != viewModel.SeoName)
                {
                    var repo = MixPageContentViewModel.GetRepository(_cmsUow, _cacheService);
                    var exists = repo.GetListQuery(m => m.SeoName == seoName && m.TenantId == viewModel.TenantId && m.Id != id).Any();
                    if (exists)
                        throw new McpException($"A page with SEO name '{seoName}' already exists.");
                }
                if (!string.IsNullOrWhiteSpace(title)) viewModel.Title = title;
                if (!string.IsNullOrWhiteSpace(content)) viewModel.Content = content;
                if (!string.IsNullOrWhiteSpace(seoName)) viewModel.SeoName = seoName;
                if (excerpt != null) viewModel.Excerpt = excerpt;
                if (status.HasValue) viewModel.Status = (MixContentStatus)status.Value;
                viewModel.LastModified = DateTime.UtcNow;
                await viewModel.SaveAsync(ct);
                await viewModel.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Page content with ID {id} updated successfully", Data = viewModel }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdatePageContent");
        }

        /// <summary>
        /// Delete page content by ID
        /// </summary>
        [McpServerTool, Description("Delete page content by its ID")]
        public async Task<string> DeletePageContent(
            [Description("Page content ID to delete")] int id,
            [Description("Confirm deletion with 'YES' (case sensitive)")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new McpException("ID must be greater than 0.");
            if (confirmDelete != "YES")
                throw new McpException("To delete page content, you must confirm by setting confirmDelete to 'YES' (case sensitive).");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting page content with ID: {Id}", id);
                var viewModel = await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService)
                    .GetFirstAsync(m => m.Id == id, ct);
                if (viewModel == null)
                    throw new McpException($"Page content with ID {id} not found.");
                await MixPageContentViewModel.GetRepository(_cmsUow, _cacheService).DeleteAsync(id, ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Page content with ID {id} deleted successfully", Id = id }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeletePageContent");
        }

        /// <summary>
        /// List page contents with filtering and pagination
        /// </summary>
        [McpServerTool, Description("List page contents with optional filtering and pagination")]
        public async Task<string> ListPageContents(
            [Description("Search keyword")] string? keyword = null,
            [Description("Filter by status (0=Preview, 1=Published, 2=Draft)")] int? status = null,
            [Description("Filter by tenant ID")] int? tenantId = null,
            [Description("Page index (0-based)")] int pageIndex = 0,
            [Description("Page size")] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 0)
                throw new McpException("Page index must be 0 or greater.");
            if (pageSize <= 0 || pageSize > 100)
                throw new McpException("Page size must be between 1 and 100.");

            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing page contents with keyword: {Keyword}, status: {Status}", keyword, status);
                var repo = MixPageContentViewModel.GetRepository(_cmsUow, _cacheService);
                var query = repo.GetListQuery(m => true, ct);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(m => m.Title.Contains(keyword) || m.Content.Contains(keyword) || m.SeoName.Contains(keyword));
                }
                if (status.HasValue)
                    query = query.Where(m => (int)m.Status == status.Value);
                if (tenantId.HasValue)
                    query = query.Where(m => m.TenantId == tenantId.Value);
                var totalCount = query.Count();
                var items = query
                    .OrderByDescending(m => m.LastModified)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToList();
                var viewModels = items.Select(e => new MixPageContentViewModel(e, _cmsUow)).ToList();
                foreach (var vm in viewModels)
                {
                    await vm.ExpandView(cancellationToken);
                }
                return ReflectionHelper.ParseObject(new
                {
                    Success = true,
                    Data = new
                    {
                        Items = viewModels,
                        TotalCount = totalCount,
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    }
                }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListPageContents");
        }
    }
}