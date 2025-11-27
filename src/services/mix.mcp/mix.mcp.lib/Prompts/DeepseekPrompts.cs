using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Prompts
{

    [McpServerPromptType]
    public class DeepseekPrompts
    {
        [McpServerPrompt, Description("CreateMixDbData prompt for data analysis")]
        public static ChatMessage CreateAnalysisPrompt(
            [Description("Analysis type")] string analysisType,
            [Description("Data to analyze")] string data) =>
            new(ChatRole.User, $"Please {analysisType} the following data:\n\n{data}");

        [McpServerPrompt, Description("CreateMixDbData prompt for text summarization")]
        public static ChatMessage CreateSummaryPrompt(
            [Description("Text to summarize")] string text) =>
            new(ChatRole.User, $"Please summarize the following text in 3-5 sentences:\n\n{text}");

        [McpServerPrompt, Description("CreateMixDbData prompt for sentiment analysis")]
        public static ChatMessage CreateSentimentPrompt(
            [Description("Text for sentiment analysis")] string text) =>
            new(ChatRole.User, $"Analyze the sentiment of the following text (positive/negative/neutral):\n\n{text}");
    }
}
