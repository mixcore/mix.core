using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.ViewModels;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MixPostContentTool : BaseMcpTool
    {
        public MixPostContentTool(AppSettingsService appSettingsService, UnitOfWorkInfo<MixCmsContext> cmsUow, ILogger<MixPostContentTool> logger)
            : base(appSettingsService, cmsUow, logger) { }

        [McpServerTool, Description("Create a new post content")]
        public async Task<string> CreatePostContent(
            [Description("Post title")] string title,
            [Description("Post content body")] string content,
            [Description("SEO name for the post")] string seoName,
            [Description("Post excerpt/description")] string? excerpt = null,
            [Description("Culture code (e.g., 'en-us')")] string? culture = null,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new McpException("Title cannot be empty.");
            if (string.IsNullOrWhiteSpace(content)) throw new McpException("Content cannot be empty.");
            if (string.IsNullOrWhiteSpace(seoName)) throw new McpException("SEO name cannot be empty.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating post content with title: {Title}, SEO name: {SeoName}", title, seoName);
                var repo = MixPostContentViewModel.GetRepository(_cmsUow, null);
                var exists = repo.GetListQuery(m => m.SeoName == seoName && m.TenantId == tenantId, ct).Any();
                if (exists) throw new McpException($"A post with SEO name '{seoName}' already exists.");
                var vm = new MixPostContentViewModel(_cmsUow)
                {
                    Title = title,
                    Content = content,
                    SeoName = seoName,
                    Excerpt = excerpt ?? string.Empty,
                    TenantId = tenantId,
                    Specificulture = culture ?? _appSettingsService.AppSettings.DefaultCulture,
                    CreatedDateTime = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Status = MixContentStatus.Published
                };
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Post content '{title}' created successfully", Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreatePostContent");
        }

        [McpServerTool, Description("Get post content by ID")]
        public async Task<string> GetPostContent(
            [Description("Post content ID")] int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Retrieving post content with ID: {Id}", id);
                var vm = await MixPostContentViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Post content with ID {id} not found.");
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetPostContent");
        }

        [McpServerTool, Description("Update post content")]
        public async Task<string> UpdatePostContent(
            [Description("Post content ID to update")] int id,
            [Description("New post title")] string? title = null,
            [Description("New post content body")] string? content = null,
            [Description("New SEO name")] string? seoName = null,
            [Description("New post excerpt/description")] string? excerpt = null,
            [Description("New content status (0=Preview, 1=Published, 2=Draft)")] MixContentStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating post content with ID: {Id}", id);
                var vm = await MixPostContentViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Post content with ID {id} not found.");
                if (!string.IsNullOrWhiteSpace(seoName) && seoName != vm.SeoName)
                {
                    var repo = MixPostContentViewModel.GetRepository(_cmsUow, null);
                    var exists = repo.GetListQuery(m => m.SeoName == seoName && m.TenantId == vm.TenantId && m.Id != id, ct).Any();
                    if (exists) throw new McpException($"A post with SEO name '{seoName}' already exists.");
                }
                if (!string.IsNullOrWhiteSpace(title)) vm.Title = title;
                if (!string.IsNullOrWhiteSpace(content)) vm.Content = content;
                if (!string.IsNullOrWhiteSpace(seoName)) vm.SeoName = seoName;
                if (excerpt != null) vm.Excerpt = excerpt;
                if (status.HasValue) vm.Status = status.Value;
                vm.LastModified = DateTime.UtcNow;
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Post content with ID {id} updated successfully", Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdatePostContent");
        }

        [McpServerTool, Description("Delete post content by ID")]
        public async Task<string> DeletePostContent(
            [Description("Post content ID to delete")] int id,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete post content, confirmDelete must be 'YES'.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting post content with ID: {Id}", id);
                var repo = MixPostContentViewModel.GetRepository(_cmsUow, null);
                var vm = await repo.GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Post content with ID {id} not found.");
                await repo.DeleteAsync(id, ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Post content with ID {id} deleted successfully", Id = id }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeletePostContent");
        }

        [McpServerTool, Description("List post contents with optional filtering")]
        public async Task<string> ListPostContents(
            [Description("Search keyword")] string? keyword = null,
            [Description("Filter by status (0=Preview, 1=Published, 2=Draft)")] int? status = null,
            [Description("Filter by tenant ID")] int? tenantId = null,
            [Description("Page index")] int pageIndex = 0,
            [Description("Page size")] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 0) throw new McpException("Page index must be 0 or greater.");
            if (pageSize <= 0 || pageSize > 100) throw new McpException("Page size must be between 1 and 100.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing post contents with keyword: {Keyword}, status: {Status}", keyword, status);
                var repo = MixPostContentViewModel.GetRepository(_cmsUow, null);
                var query = repo.GetListQuery(m => true, ct);
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(m => m.Title.Contains(keyword) || m.Content.Contains(keyword) || m.SeoName.Contains(keyword));
                if (status.HasValue)
                    query = query.Where(m => (int)m.Status == status.Value);
                if (tenantId.HasValue)
                    query = query.Where(m => m.TenantId == tenantId.Value);
                var totalCount = query.Count();
                var items = query.OrderByDescending(m => m.LastModified).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var vms = items.Select(e => new MixPostContentViewModel(e, _cmsUow)).ToList();
                foreach (var vm in vms) await vm.ExpandView(cancellationToken);
                return ReflectionHelper.ParseObject(new { Success = true, Data = new { Items = vms, TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) } }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListPostContents");
        }
    }
}
