using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Resources;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tool for accessing MCP resources
    /// </summary>
    [McpServerToolType]
    public class ResourceTool
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly ILogger<ResourceTool> _logger;

        /// <summary>
        /// Initializes a new instance of the ResourceTool class
        /// </summary>
        /// <param name="resourceLoader">The resource loader service</param>
        /// <param name="logger">The logger</param>
        public ResourceTool(ResourceLoader resourceLoader, ILogger<ResourceTool> logger)
        {
            _resourceLoader = resourceLoader;
            _logger = logger;
        }

        /// <summary>
        /// Gets a resource by section and key
        /// </summary>
        /// <param name="section">The section name</param>
        /// <param name="key">The resource key</param>
        /// <returns>The resource value</returns>
        [McpServerTool, Description("Get a resource by section and key")]
        public string GetResource(
            [Description("Section name (e.g., Prompts, Error Messages, Server Settings)")] string section,
            [Description("Resource key within the section")] string key)
        {
            _logger.LogInformation("Getting resource for section {Section} with key {Key}", section, key);
            return _resourceLoader.GetResource(section, key);
        }

        /// <summary>
        /// Gets all resources in a section
        /// </summary>
        /// <param name="section">The section name</param>
        /// <returns>A JSON string containing all resources in the section</returns>
        [McpServerTool, Description("Get all resources in a section")]
        public string GetSection(
            [Description("Section name (e.g., Prompts, Error Messages, Server Settings)")] string section)
        {
            _logger.LogInformation("Getting all resources for section {Section}", section);
            var sectionData = _resourceLoader.GetSection(section);
            return JsonSerializer.Serialize(sectionData);
        }

        /// <summary>
        /// Lists all available resource sections
        /// </summary>
        /// <returns>A JSON string containing all section names</returns>
        [McpServerTool, Description("List all available resource sections")]
        public string ListSections()
        {
            _logger.LogInformation("Listing all resource sections");
            var sections = _resourceLoader.GetSections().ToArray();
            return JsonSerializer.Serialize(sections);
        }

        /// <summary>
        /// Format a resource with parameters
        /// </summary>
        /// <param name="section">The section name</param>
        /// <param name="key">The resource key</param>
        /// <param name="parameters">Comma-separated parameters for formatting</param>
        /// <returns>The formatted resource</returns>
        [McpServerTool, Description("Format a resource with parameters")]
        public string FormatResource(
            [Description("Section name (e.g., Prompts, Error Messages)")] string section,
            [Description("Resource key within the section")] string key,
            [Description("Comma-separated parameters for formatting")] string parameters)
        {
            _logger.LogInformation("Formatting resource for section {Section} with key {Key}", section, key);

            var paramArray = parameters.Split(',')
                .Select(p => p.Trim())
                .Cast<object>()
                .ToArray();

            return _resourceLoader.FormatResource(section, key, paramArray);
        }
    }
}