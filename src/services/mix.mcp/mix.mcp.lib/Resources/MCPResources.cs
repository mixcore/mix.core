using System;
using System.Collections.Generic;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Mix.MCP.Lib.Resources
{
    /// <summary>
    /// Resources for Model Context Protocol used in Mixcore
    /// </summary>
    public static class MCPResources
    {
        /// <summary>
        /// List of content types supported in MCP
        /// </summary>
        public static readonly Dictionary<string, string> ContentTypes = new()
        {
            { "text", "Text content" },
            { "image", "Image content" },
            { "audio", "Audio content" },
            { "video", "Video content" },
            { "application/json", "JSON data" },
        };

        /// <summary>
        /// Standard prompt templates
        /// </summary>
        public static class StandardPrompts
        {
            public static readonly string SummarizeTemplate = "Summarize the following content in {0} sentences: \n\n{1}";
            public static readonly string AnalyzeTemplate = "Analyze the {0} of the following data: \n\n{1}";
            public static readonly string TranslateTemplate = "Translate the following content from {0} to {1}: \n\n{2}";
        }

        /// <summary>
        /// Information about standard tools
        /// </summary>
        public static class StandardTools
        {
            public static readonly string EchoDescription = "Echo back the received LLMMessage";
            public static readonly string SummarizeDescription = "Summarize the provided content";
            public static readonly string AnalyzeDescription = "Analyze the provided data";
        }

        /// <summary>
        /// Standard error messages
        /// </summary>
        public static class ErrorMessages
        {
            public static readonly string MissingArgument = "Missing required parameter: {0}";
            public static readonly string InvalidArgumentType = "Invalid data type for parameter {0}";
            public static readonly string ToolNotFound = "Tool not found: {0}";
            public static readonly string PromptNotFound = "Prompt not found: {0}";
            public static readonly string ServerError = "Server error: {0}";
        }

        /// <summary>
        /// DefaultBaseUrl settings for MCP server
        /// </summary>
        public static class ServerDefaults
        {
            public static readonly string ServerName = "Mixcore MCP Server";
            public static readonly string ServerVersion = "1.0.0";
            public static readonly int DefaultTimeout = 30000; // 30 seconds
            public static readonly int MaxOutputTokens = 4096;
            public static readonly float DefaultTemperature = 0.7f;
        }
    }
}