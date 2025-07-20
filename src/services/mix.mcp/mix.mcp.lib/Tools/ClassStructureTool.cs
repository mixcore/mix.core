using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Mix.Heart.Helpers;

namespace Mix.MCP.Lib.Tools
{
    [McpServerToolType]
    public class ClassStructureTool
    {
        private readonly ILogger<ClassStructureTool> _logger;
        public ClassStructureTool(ILogger<ClassStructureTool> logger)
        {
            _logger = logger;
        }

        [McpServerTool, Description("Get the structure of a class by name (properties, methods, attributes)")]
        public string GetClassStructure(
            [Description("Full class name (e.g. Mix.Lib.ViewModels.MixPostContentViewModel)")] string className)
        {
            _logger.LogInformation("Getting class structure for {ClassName}", className);
            var type = Type.GetType(className);
            if (type == null)
            {
                return ReflectionHelper.ParseObject(new { Success = false, Message = $"Class '{className}' not found." }).ToString(Newtonsoft.Json.Formatting.None);
            }
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new {
                    Name = p.Name,
                    Type = p.PropertyType.Name,
                    Attributes = p.GetCustomAttributes().Select(a => a.GetType().Name).ToList()
                }).ToList();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName)
                .Select(m => new {
                    Name = m.Name,
                    ReturnType = m.ReturnType.Name,
                    Parameters = m.GetParameters().Select(param => new { param.Name, Type = param.ParameterType.Name }).ToList(),
                    Attributes = m.GetCustomAttributes().Select(a => a.GetType().Name).ToList()
                }).ToList();
            var result = new {
                Success = true,
                ClassName = type.FullName,
                Properties = properties,
                Methods = methods
            };
            return ReflectionHelper.ParseObject(result).ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
