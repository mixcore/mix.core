using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mix.MCP.Lib.Tools
{
    public static class ToolDiscovery
    {
        public static IReadOnlyList<(string MethodName, string Description)> SupportedPromptToolActions { get; }

        static ToolDiscovery()
        {
            SupportedPromptToolActions = DiscoverSupportedPromptToolActions();
        }

        private static List<(string MethodName, string Description)> DiscoverSupportedPromptToolActions()
        {
            var actions = new List<(string MethodName, string Description)>();
            var assembly = typeof(ToolDiscovery).Assembly;

            foreach (var type in assembly.GetTypes())
            {
                // Only consider classes with [McpServerToolType] attribute
                if (type.GetCustomAttribute<McpServerToolTypeAttribute>() == null)
                    continue;

                foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                {
                    // Only consider methods with [McpServerTool] attribute
                    if (method.GetCustomAttribute<McpServerToolAttribute>() == null)
                        continue;

                    // Try to get a [Description] attribute if present
                    var descAttr = method.GetCustomAttribute<DescriptionAttribute>();
                    string description = descAttr != null
                        ? descAttr.Description
                        : string.Join(", ", method.GetParameters()
                            .Where(p => !p.HasDefaultValue)
                            .Select(p => $"{p.Name} ({p.ParameterType.Name})"));
                    var parameters = method.GetParameters().Where(m => !m.HasDefaultValue).Select(m => $"{m.Name} ({m.ParameterType})").ToList();

                    actions.Add((method.Name, $"{description}, parameters: {string.Join(", ", parameters)}"));
                }
            }
            return actions;
        }
    }
}