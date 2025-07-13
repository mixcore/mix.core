using Microsoft.Extensions.Logging;
using Mix.Heart.UnitOfWork;
using System.ComponentModel;
using Mix.Database.Entities.Cms;
using Mix.Constant.Enums;
using Mix.Heart.Enums;
using Mix.Portal.Domain.ViewModels;
using ModelContextProtocol.Server;
using ModelContextProtocol;
using Mix.Heart.Helpers;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MixModuleContentTool : BaseMcpTool
    {
        public MixModuleContentTool(UnitOfWorkInfo<MixCmsContext> cmsUow, ILogger<MixModuleContentTool> logger)
            : base(cmsUow, logger) { }

        [McpServerTool, Description("Create a new module content")]
        public async Task<string> CreateModuleContent(
            [Description("Module title")] string title,
            [Description("System name")] string systemName,
            [Description("Module excerpt/description")] string? excerpt = null,
            [Description("Module type")] int type = 0,
            [Description("Page size")] int? pageSize = null,
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new McpException("Title cannot be empty.");
            if (string.IsNullOrWhiteSpace(systemName)) throw new McpException("SystemName cannot be empty.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating module content with title: {Title}, SystemName: {SystemName}", title, systemName);
                var repo = MixModuleContentViewModel.GetRepository(_cmsUow, null);
                var exists = repo.GetListQuery(m => m.SystemName == systemName && m.TenantId == tenantId, ct).Any();
                if (exists) throw new McpException($"A module with system name '{systemName}' already exists.");
                var vm = new MixModuleContentViewModel(_cmsUow)
                {
                    Title = title,
                    SystemName = systemName,
                    Excerpt = excerpt ?? string.Empty,
                    TenantId = tenantId,
                    PageSize = pageSize,
                    Type = (MixModuleType)type,
                    CreatedDateTime = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Status = MixContentStatus.Published
                };
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Module content '{title}' created successfully", Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateModuleContent");
        }

        [McpServerTool, Description("Get module content by ID")]
        public async Task<string> GetModuleContent(
            [Description("Module content ID")] int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Retrieving module content with ID: {Id}", id);
                var vm = await MixModuleContentViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Module content with ID {id} not found.");
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetModuleContent");
        }

        [McpServerTool, Description("Update module content")]
        public async Task<string> UpdateModuleContent(
            [Description("Module content ID to update")] int id,
            [Description("New module title")] string? title = null,
            [Description("New system name")] string? systemName = null,
            [Description("New excerpt/description")] string? excerpt = null,
            [Description("New module type")] int? type = null,
            [Description("New page size")] int? pageSize = null,
            [Description("New status (0=Preview, 1=Published, 2=Draft)")] int? status = null,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating module content with ID: {Id}", id);
                var vm = await MixModuleContentViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Module content with ID {id} not found.");
                if (!string.IsNullOrWhiteSpace(systemName) && systemName != vm.SystemName)
                {
                    var repo = MixModuleContentViewModel.GetRepository(_cmsUow, null);
                    var exists = repo.GetListQuery(m => m.SystemName == systemName && m.TenantId == vm.TenantId && m.Id != id, ct).Any();
                    if (exists) throw new McpException($"A module with system name '{systemName}' already exists.");
                }
                if (!string.IsNullOrWhiteSpace(title)) vm.Title = title;
                if (!string.IsNullOrWhiteSpace(systemName)) vm.SystemName = systemName;
                if (excerpt != null) vm.Excerpt = excerpt;
                if (type.HasValue) vm.Type = (MixModuleType)type.Value;
                if (pageSize.HasValue) vm.PageSize = pageSize;
                if (status.HasValue) vm.Status = (MixContentStatus)status.Value;
                vm.LastModified = DateTime.UtcNow;
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Module content with ID {id} updated successfully", Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdateModuleContent");
        }

        [McpServerTool, Description("Delete module content by ID")]
        public async Task<string> DeleteModuleContent(
            [Description("Module content ID to delete")] int id,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete module content, confirmDelete must be 'YES'.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting module content with ID: {Id}", id);
                var repo = MixModuleContentViewModel.GetRepository(_cmsUow, null);
                var vm = await repo.GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Module content with ID {id} not found.");
                await repo.DeleteAsync(id, ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Module content with ID {id} deleted successfully", Id = id }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeleteModuleContent");
        }

        [McpServerTool, Description("List module contents with optional filtering")]
        public async Task<string> ListModuleContents(
            [Description("Search keyword")] string? keyword = null,
            [Description("Filter by type")] int? type = null,
            [Description("Filter by tenant ID")] int? tenantId = null,
            [Description("Page index")] int pageIndex = 0,
            [Description("Page size")] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 0) throw new McpException("Page index must be 0 or greater.");
            if (pageSize <= 0 || pageSize > 100) throw new McpException("Page size must be between 1 and 100.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing module contents with keyword: {Keyword}, type: {Type}", keyword, type);
                var repo = MixModuleContentViewModel.GetRepository(_cmsUow, null);
                var query = repo.GetListQuery(m => true, ct);
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(m => m.Title.Contains(keyword) || m.SystemName.Contains(keyword));
                if (type.HasValue)
                    query = query.Where(m => (int)m.Type == type.Value);
                if (tenantId.HasValue)
                    query = query.Where(m => m.TenantId == tenantId.Value);
                var totalCount = query.Count();
                var items = query.OrderByDescending(m => m.LastModified).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var vms = items.Select(e => new MixModuleContentViewModel(e, _cmsUow)).ToList();
                foreach (var vm in vms) await vm.ExpandView(cancellationToken);
                return ReflectionHelper.ParseObject(new { Success = true, Data = new { Items = vms, TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) } }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListModuleContents");
        }
    }
}
