using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    [McpServerPromptType]
    public class DatabaseAnalysisPrompt
    {
        [McpServerPrompt, Description("CreateMixDbData prompt for data analysis")]
        public static ChatMessage CreateAnalysisTablePrompt(
            [Description("Table name")] string tableName,
            [Description("Analysis type")] string analysisType,
            [Description("Additional context")] string context = "") =>
            new(ChatRole.User, $"""
                Please analyze the data from table '{tableName}' with the following requirements:
                {context}

                Analysis Type: {analysisType}

                Requirements:
                1. Provide descriptive statistics
                2. Identify trends and patterns
                3. Check for data quality issues
                4. Suggest relevant visualizations
                5. Highlight key insights
                6. Provide recommendations
                7. Include data validation checks
                8. Consider business context
                9. Check for anomalies
                10. Provide actionable insights

                Please provide:
                1. Summary of findings
                2. Key metrics and statistics
                3. Visual representation suggestions
                4. Recommendations for next steps
                """);

        [McpServerPrompt, Description("CreateMixDbData prompt for data exploration")]
        public static ChatMessage CreateExplorationTablePrompt(
            [Description("Table name")] string tableName,
            [Description("Exploration goals")] string goals) =>
            new(ChatRole.User, $"""
                Please explore the data from table '{tableName}' with the following goals:
                {goals}

                Requirements:
                1. Understand data structure
                2. Identify key fields
                3. Check data distributions
                4. Find relationships between fields
                5. Identify potential use cases
                6. Suggest data quality improvements
                7. Recommend data modeling approaches
                8. Consider performance implications
                9. Check for data completeness
                10. Identify data integration opportunities

                Please provide:
                1. Data structure overview
                2. Key findings
                3. Potential use cases
                4. Recommendations for data management
                """);
    }
} 