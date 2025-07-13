using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.Lib.ViewModels;
using Mix.Portal.Domain.Interfaces;
using Mix.Portal.Domain.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class MixTemplateTool : BaseMcpTool
    {
        private IThemeService _themeService;
        protected readonly IMixTenantService _mixTenantService;
        public MixTemplateTool(
            IMixTenantService mixTenantService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ILogger<MixTemplateTool> logger,
            IThemeService themeService)
            : base(cmsUow, logger)
        {
            _mixTenantService = mixTenantService;
            _themeService = themeService;
        }

        [McpServerTool, Description("Create a new template")] 
        public async Task<string> CreateTemplate(
            [Description("Template file name")] string fileName,
            [Description("Template content")] string content,
            [Description("Theme ID")] int mixThemeId,
            [Description("Folder type (0=Layouts, 1=Pages, 2=Modules, 3=Forms, 4=Edms, 5=Posts, 6=Widgets, 7=Masters)")] int folderType = 1,
            [Description("Extension")] string extension = "cshtml",
            [Description("Tenant ID")] int tenantId = 1,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new McpException("FileName cannot be empty.");
            if (string.IsNullOrWhiteSpace(content)) throw new McpException("Content cannot be empty.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Creating template: {FileName}", fileName);
                var repo = MixTemplateViewModel.GetRepository(_cmsUow, null);
                var exists = repo.GetListQuery(m => m.FileName == fileName && m.MixThemeId == mixThemeId && (int)m.FolderType == folderType, ct).Any();
                if (exists) throw new McpException($"A template with file name '{fileName}' already exists in this theme/folder.");
                var currentTenant = _mixTenantService.GetDefaultTenant().GetAwaiter().GetResult();
                var activeTheme = await _themeService.GetActiveTheme();
                var vm = new MixTemplateViewModel(_cmsUow)
                {
                    FileName = fileName,
                    Content = content,
                    MixThemeId = mixThemeId,
                    FolderType = (MixTemplateFolderType)folderType,
                    FileFolder = $"{MixFolders.TemplatesFolder}/{currentTenant.SystemName}/{activeTheme.SystemName}/{(MixTemplateFolderType)folderType}",
                    Extension = extension,
                    TenantId = tenantId
                };
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                SaveTemplateFile(vm);
                return ReflectionHelper.ParseObject(
                    new { 
                            Success = true, 
                            Message = $"Template '{fileName}' created successfully", 
                            Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "CreateTemplate");
        }

        [McpServerTool, Description("Get template by ID")]
        public async Task<string> GetTemplate(
            [Description("Template ID")] int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Retrieving template with ID: {Id}", id);
                var vm = await MixTemplateViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Template with ID {id} not found.");
                await vm.ExpandView(ct);
                return ReflectionHelper.ParseObject(new { Success = true, Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "GetTemplate");
        }

        [McpServerTool, Description("Update template")]
        public async Task<string> UpdateTemplate(
            [Description("Template ID")] int id,
            [Description("New file name")] string? fileName = null,
            [Description("New content")] string? content = null,
            [Description("New extension")] string? extension = null,
            [Description("New folder type")] int? folderType = null,
            [Description("New theme ID")] int? mixThemeId = null,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Updating template with ID: {Id}", id);
                var vm = await MixTemplateViewModel.GetRepository(_cmsUow, null).GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Template with ID {id} not found.");
                if (!string.IsNullOrWhiteSpace(fileName)) vm.FileName = fileName;
                if (!string.IsNullOrWhiteSpace(content)) vm.Content = content;
                if (!string.IsNullOrWhiteSpace(extension)) vm.Extension = extension;
                if (folderType.HasValue) vm.FolderType = (MixTemplateFolderType)folderType.Value;
                if (mixThemeId.HasValue) vm.MixThemeId = mixThemeId.Value;
                await vm.SaveAsync(ct);
                await vm.ExpandView(ct);
                SaveTemplateFile(vm);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Template with ID {id} updated successfully", Data = vm }).ToString(Newtonsoft.Json.Formatting.None);
            }, "UpdateTemplate");
        }

        [McpServerTool, Description("Delete template by ID")]
        public async Task<string> DeleteTemplate(
            [Description("Template ID")] int id,
            [Description("Confirm deletion with 'YES'")] string confirmDelete = "",
            CancellationToken cancellationToken = default)
        {
            if (id <= 0) throw new McpException("ID must be greater than 0.");
            if (confirmDelete != "YES") throw new McpException("To delete template, confirmDelete must be 'YES'.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Deleting template with ID: {Id}", id);
                var repo = MixTemplateViewModel.GetRepository(_cmsUow, null);
                var vm = await repo.GetFirstAsync(m => m.Id == id, ct);
                if (vm == null) throw new McpException($"Template with ID {id} not found.");
                await repo.DeleteAsync(id, ct);
                return ReflectionHelper.ParseObject(new { Success = true, Message = $"Template with ID {id} deleted successfully", Id = id }).ToString(Newtonsoft.Json.Formatting.None);
            }, "DeleteTemplate");
        }

        [McpServerTool, Description("List templates with optional filtering")]
        public async Task<string> ListTemplates(
            [Description("Search keyword")] string? keyword = null,
            [Description("Theme ID")] int? mixThemeId = null,
            [Description("Folder type")] int? folderType = null,
            [Description("Page index")] int pageIndex = 0,
            [Description("Page size")] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 0) throw new McpException("Page index must be 0 or greater.");
            if (pageSize <= 0 || pageSize > 100) throw new McpException("Page size must be between 1 and 100.");
            return await ExecuteWithExceptionHandlingAsync(async (ct) =>
            {
                _logger.LogInformation("Listing templates with keyword: {Keyword}", keyword);
                var repo = MixTemplateViewModel.GetRepository(_cmsUow, null);
                var query = repo.GetListQuery(m => true, ct);
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(m => m.FileName.Contains(keyword) || m.Content.Contains(keyword));
                if (mixThemeId.HasValue)
                    query = query.Where(m => m.MixThemeId == mixThemeId.Value);
                if (folderType.HasValue)
                    query = query.Where(m => (int)m.FolderType == folderType.Value);
                var totalCount = query.Count();
                var items = query.OrderByDescending(m => m.LastModified).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var vms = items.Select(e => new MixTemplateViewModel(e, _cmsUow)).ToList();
                foreach (var vm in vms) await vm.ExpandView(cancellationToken);
                return ReflectionHelper.ParseObject(new { Success = true, Data = new { Items = vms, TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) } }).ToString(Newtonsoft.Json.Formatting.None);
            }, "ListTemplates");
        }

        private void SaveTemplateFile(MixTemplateViewModel vm)
        {
            MixFileHelper.SaveFile(new FileModel()
            {
                Content = vm.Content,
                Filename = vm.FileName,
                Extension = vm.Extension,
                FileFolder = vm.FileFolder
            });
        }

    }
}
