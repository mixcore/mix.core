using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Lib.ViewModels;
using Mix.Lib.Dtos;
using Mix.Constant.Enums;
using ModelContextProtocol.Server;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// MCP tool for interacting with Mixcore CMS templates
    /// </summary>
    [McpServerToolType]
    public class MixTemplateTools : BaseMcpTool
    {
        /// <summary>
        /// Initializes a new instance of the MixTemplateTools class
        /// </summary>
        public MixTemplateTools(
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            ILogger<MixTemplateTools> logger)
            : base(cmsUow, logger)
        {
        }

        /// <summary>
        /// Get all templates with optional filtering
        /// </summary>
        [McpServerTool, Description("Get all Mixcore CMS templates with optional filtering")]
        public async Task<string> GetTemplates(
            [Description("Optional theme ID to filter by")] int? themeId = null,
            [Description("Optional folder type (0=Layouts, 1=Pages, 2=Modules, 3=Forms, 4=Edms, 5=Posts, 6=Widgets, 7=Masters)")] int? folderType = null,
            [Description("Optional keyword to search for")] string keyword = null,
            [Description("Page index (0-based)")] int pageIndex = 0,
            [Description("Page size")] int pageSize = 10)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var query = _cmsUow.Repository<MixTemplate>().GetModelListBy(m => true);

                if (themeId.HasValue)
                {
                    query = query.Where(m => m.MixThemeId == themeId.Value);
                }

                if (folderType.HasValue)
                {
                    query = query.Where(m => m.FolderType == (MixTemplateFolderType)folderType.Value);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(m => m.FileName.Contains(keyword) || m.Content.Contains(keyword));
                }

                var total = await query.CountAsync(cancellationToken);
                var templates = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var viewModels = new List<object>();
                foreach (var template in templates)
                {
                    var vm = new MixTemplateViewModel(template, _cmsUow);
                    await vm.ExpandView(cancellationToken);
                    viewModels.Add(new
                    {
                        Id = vm.Id,
                        FileName = vm.FileName,
                        Extension = vm.Extension,
                        FolderType = vm.FolderType.ToString(),
                        MixThemeName = vm.MixThemeName,
                        MixThemeId = vm.MixThemeId,
                        Content = vm.Content,
                        Scripts = vm.Scripts,
                        Styles = vm.Styles,
                        CreatedDateTime = vm.CreatedDateTime,
                        LastModified = vm.LastModified,
                        CreatedBy = vm.CreatedBy,
                        ModifiedBy = vm.ModifiedBy
                    });
                }

                return JsonConvert.SerializeObject(new
                {
                    Items = viewModels,
                    TotalItems = total,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize)
                });
            }, "GetTemplates");
        }

        /// <summary>
        /// Get a single template by ID
        /// </summary>
        [McpServerTool, Description("Get a single Mixcore CMS template by ID")]
        public async Task<string> GetTemplateById(
            [Description("Template ID")] int id)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var template = await _cmsUow.Repository<MixTemplate>().GetSingleAsync(m => m.Id == id, cancellationToken);

                if (template == null)
                {
                    throw new Exception($"Template with ID {id} not found");
                }

                var viewModel = new MixTemplateViewModel(template, _cmsUow);
                await viewModel.ExpandView(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    Id = viewModel.Id,
                    FileName = viewModel.FileName,
                    Extension = viewModel.Extension,
                    FileFolder = viewModel.FileFolder,
                    FolderType = viewModel.FolderType.ToString(),
                    MixThemeName = viewModel.MixThemeName,
                    MixThemeId = viewModel.MixThemeId,
                    Content = viewModel.Content,
                    Scripts = viewModel.Scripts,
                    Styles = viewModel.Styles,
                    CreatedDateTime = viewModel.CreatedDateTime,
                    LastModified = viewModel.LastModified,
                    CreatedBy = viewModel.CreatedBy,
                    ModifiedBy = viewModel.ModifiedBy
                });
            }, "GetTemplateById");
        }

        /// <summary>
        /// Create a new template
        /// </summary>
        [McpServerTool, Description("Create a new Mixcore CMS template")]
        public async Task<string> CreateTemplate(
            [Description("Template filename")] string fileName,
            [Description("File extension (e.g., 'cshtml')")] string extension,
            [Description("Folder type (0=Layouts, 1=Pages, 2=Modules, 3=Forms, 4=Edms, 5=Posts, 6=Widgets, 7=Masters)")] int folderType,
            [Description("Theme ID")] int themeId,
            [Description("Template content")] string content = "",
            [Description("JavaScript content")] string scripts = "",
            [Description("CSS content")] string styles = "")
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var template = new MixTemplateViewModel(_cmsUow)
                {
                    FileName = fileName,
                    Extension = extension,
                    FolderType = (MixTemplateFolderType)folderType,
                    MixThemeId = themeId,
                    Content = content,
                    Scripts = scripts,
                    Styles = styles
                };

                var result = await template.SaveAsync(cancellationToken);

                if (result <= 0)
                {
                    throw new Exception("Failed to create template");
                }

                return JsonConvert.SerializeObject(new
                {
                    Id = result,
                    Message = "Template created successfully"
                });
            }, "CreateTemplate");
        }

        /// <summary>
        /// Update an existing template
        /// </summary>
        [McpServerTool, Description("Update an existing Mixcore CMS template")]
        public async Task<string> UpdateTemplate(
            [Description("Template ID")] int id,
            [Description("Template filename")] string fileName = null,
            [Description("File extension")] string extension = null,
            [Description("Template content")] string content = null,
            [Description("JavaScript content")] string scripts = null,
            [Description("CSS content")] string styles = null)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var entity = await _cmsUow.Repository<MixTemplate>().GetSingleAsync(m => m.Id == id, cancellationToken);

                if (entity == null)
                {
                    throw new Exception($"Template with ID {id} not found");
                }

                var template = new MixTemplateViewModel(entity, _cmsUow);

                // Update only provided fields
                if (!string.IsNullOrEmpty(fileName))
                    template.FileName = fileName;
                if (!string.IsNullOrEmpty(extension))
                    template.Extension = extension;
                if (content != null)
                    template.Content = content;
                if (scripts != null)
                    template.Scripts = scripts;
                if (styles != null)
                    template.Styles = styles;

                await template.SaveAsync(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    Id = id,
                    Message = "Template updated successfully"
                });
            }, "UpdateTemplate");
        }

        /// <summary>
        /// Delete a template
        /// </summary>
        [McpServerTool, Description("Delete a Mixcore CMS template")]
        public async Task<string> DeleteTemplate(
            [Description("Template ID")] int id)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var entity = await _cmsUow.Repository<MixTemplate>().GetSingleAsync(m => m.Id == id, cancellationToken);

                if (entity == null)
                {
                    throw new Exception($"Template with ID {id} not found");
                }

                var template = new MixTemplateViewModel(entity, _cmsUow);
                await template.DeleteAsync(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    Id = id,
                    Message = "Template deleted successfully"
                });
            }, "DeleteTemplate");
        }

        /// <summary>
        /// Copy an existing template
        /// </summary>
        [McpServerTool, Description("Copy an existing Mixcore CMS template")]
        public async Task<string> CopyTemplate(
            [Description("Template ID to copy")] int id)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var entity = await _cmsUow.Repository<MixTemplate>().GetSingleAsync(m => m.Id == id, cancellationToken);

                if (entity == null)
                {
                    throw new Exception($"Template with ID {id} not found");
                }

                var template = new MixTemplateViewModel(entity, _cmsUow);
                var copyResult = await template.CopyAsync(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    OriginalId = id,
                    NewId = copyResult.Id,
                    NewFileName = copyResult.FileName,
                    Message = "Template copied successfully"
                });
            }, "CopyTemplate");
        }

        /// <summary>
        /// Get default template structure
        /// </summary>
        [McpServerTool, Description("Get default template structure for creating new templates")]
        public async Task<string> GetDefaultTemplate(
            [Description("Theme ID for the default template")] int themeId,
            [Description("Folder type (0=Layouts, 1=Pages, 2=Modules, 3=Forms, 4=Edms, 5=Posts, 6=Widgets, 7=Masters)")] int folderType)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var defaultTemplate = new MixTemplateViewModel(_cmsUow)
                {
                    MixThemeId = themeId,
                    FolderType = (MixTemplateFolderType)folderType,
                    Extension = "cshtml",
                    Content = "",
                    Scripts = "<script>\r\n\r\n</script>",
                    Styles = "<style>\r\n\r\n</style>"
                };

                await defaultTemplate.ExpandView(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    MixThemeId = defaultTemplate.MixThemeId,
                    FolderType = defaultTemplate.FolderType.ToString(),
                    Extension = defaultTemplate.Extension,
                    Content = defaultTemplate.Content,
                    Scripts = defaultTemplate.Scripts,
                    Styles = defaultTemplate.Styles,
                    FileFolder = defaultTemplate.FileFolder
                });
            }, "GetDefaultTemplate");
        }

        /// <summary>
        /// Filter templates with advanced search
        /// </summary>
        [McpServerTool, Description("Filter Mixcore CMS templates with advanced search criteria")]
        public async Task<string> FilterTemplates(
            [Description("Search criteria in JSON format. Example: {\"keyword\":\"layout\",\"themeId\":1,\"folderType\":0,\"pageIndex\":0,\"pageSize\":10}")] string searchCriteria)
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var criteria = JsonConvert.DeserializeObject<dynamic>(searchCriteria);

                if (criteria == null)
                {
                    throw new Exception("Invalid search criteria format");
                }

                var query = _cmsUow.Repository<MixTemplate>().GetModelListBy(m => true);

                // Apply filters based on criteria
                if (criteria.themeId != null)
                {
                    query = query.Where(m => m.MixThemeId == (int)criteria.themeId);
                }

                if (criteria.folderType != null)
                {
                    query = query.Where(m => m.FolderType == (MixTemplateFolderType)(int)criteria.folderType);
                }

                if (criteria.keyword != null && !string.IsNullOrEmpty((string)criteria.keyword))
                {
                    string keyword = (string)criteria.keyword;
                    query = query.Where(m => m.FileName.Contains(keyword) || m.Content.Contains(keyword));
                }

                int pageIndex = criteria.pageIndex ?? 0;
                int pageSize = criteria.pageSize ?? 10;

                var total = await query.CountAsync(cancellationToken);
                var templates = await query
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return JsonConvert.SerializeObject(new
                {
                    Items = templates.Select(t => new
                    {
                        Id = t.Id,
                        FileName = t.FileName,
                        Extension = t.Extension,
                        FolderType = t.FolderType.ToString(),
                        MixThemeName = t.MixThemeName,
                        MixThemeId = t.MixThemeId,
                        CreatedDateTime = t.CreatedDateTime,
                        LastModified = t.LastModified
                    }),
                    TotalItems = total,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize)
                });
            }, "FilterTemplates");
        }

        /// <summary>
        /// List all available themes for template creation
        /// </summary>
        [McpServerTool, Description("List all available themes for template creation")]
        public async Task<string> GetAvailableThemes()
        {
            return await ExecuteWithExceptionHandlingAsync(async cancellationToken =>
            {
                var themes = await _cmsUow.Repository<MixTheme>().GetModelListByAsync(
                    t => true, // Remove the status filter for now since we can't find MixContentStatus
                    cancellationToken: cancellationToken);

                return JsonConvert.SerializeObject(themes.Select(t => new
                {
                    Id = t.Id,
                    SystemName = t.SystemName,
                    Title = t.Title,
                    CreatedDateTime = t.CreatedDateTime
                }));
            }, "GetAvailableThemes");
        }

        /// <summary>
        /// Get template folder types information
        /// </summary>
        [McpServerTool, Description("Get information about template folder types")]
        public string GetTemplateFolderTypes()
        {
            var folderTypes = Enum.GetValues<MixTemplateFolderType>()
                .Select(ft => new
                {
                    Value = (int)ft,
                    Name = ft.ToString(),
                    Description = GetFolderTypeDescription(ft)
                });

            return JsonConvert.SerializeObject(folderTypes);
        }

        private static string GetFolderTypeDescription(MixTemplateFolderType folderType)
        {
            return folderType switch
            {
                MixTemplateFolderType.Layouts => "Layout templates that define the overall structure of pages",
                MixTemplateFolderType.Pages => "Page templates for specific pages",
                MixTemplateFolderType.Modules => "Module templates for specific functionality",
                MixTemplateFolderType.Forms => "Form templates for user input",
                MixTemplateFolderType.Edms => "Electronic Document Management System templates",
                MixTemplateFolderType.Posts => "Post templates for blog/news content",
                MixTemplateFolderType.Widgets => "Widget templates for reusable components",
                MixTemplateFolderType.Masters => "Master templates for hierarchical structures",
                _ => "Unknown folder type"
            };
        }
    }
}