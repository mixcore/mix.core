using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Resources
{
    /// <summary>
    /// Class that loads MCP resources from text file
    /// </summary>
    public class ResourceLoader
    {
        private readonly ILogger<ResourceLoader> _logger;
        private readonly Dictionary<string, Dictionary<string, string>> _resources = new();

        /// <summary>
        /// Initialize ResourceLoader object
        /// </summary>
        /// <param name="logger">Logger</param>
        public ResourceLoader(ILogger<ResourceLoader> logger)
        {
            _logger = logger;
            LoadResourcesFromEmbeddedFile();
        }

        /// <summary>
        /// Get resource value by section and key
        /// </summary>
        /// <param name="section">Section name</param>
        /// <param name="key">Key</param>
        /// <returns>Resource value</returns>
        public string GetResource(string section, string key)
        {
            if (_resources.TryGetValue(section, out var sectionValues))
            {
                if (sectionValues.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            _logger.LogWarning("Resource not found for section {Section} with key {Key}", section, key);
            return $"[{section}.{key}]";
        }

        /// <summary>
        /// Get all resources in a section
        /// </summary>
        /// <param name="section">Section name</param>
        /// <returns>Dictionary containing key-value pairs in the section</returns>
        public Dictionary<string, string> GetSection(string section)
        {
            if (_resources.TryGetValue(section, out var sectionValues))
            {
                return new Dictionary<string, string>(sectionValues);
            }

            _logger.LogWarning("Resource section not found: {Section}", section);
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Get list of all sections
        /// </summary>
        /// <returns>List of section names</returns>
        public IEnumerable<string> GetSections()
        {
            return _resources.Keys;
        }

        /// <summary>
        /// Format a resource string with parameters
        /// </summary>
        /// <param name="section">Section name</param>
        /// <param name="key">Key</param>
        /// <param name="args">Parameters to replace</param>
        /// <returns>Formatted string</returns>
        public string FormatResource(string section, string key, params object[] args)
        {
            var template = GetResource(section, key);
            return string.Format(template, args);
        }

        private void LoadResourcesFromEmbeddedFile()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "Mix.MCP.Lib.Resources.MixCoreResources.txt";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    _logger.LogError("Embedded resource not found: {ResourceName}", resourceName);
                    return;
                }

                using var reader = new StreamReader(stream);
                var currentSection = string.Empty;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                    {
                        continue;
                    }

                    if (line.StartsWith("##"))
                    {
                        currentSection = line.Substring(2).Trim();
                        if (!_resources.ContainsKey(currentSection))
                        {
                            _resources[currentSection] = new Dictionary<string, string>();
                        }
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currentSection))
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            _resources[currentSection][key] = value;
                        }
                    }
                }

                _logger.LogInformation("Loaded {SectionCount} resource sections with a total of {ResourceCount} items",
                    _resources.Count, _resources.Sum(section => section.Value.Count));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading resources from embedded file");
            }
        }
    }
}