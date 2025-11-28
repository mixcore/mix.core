using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    [McpServerPromptType]
    public class IoTHealthDataPrompt
    {
        [McpServerPrompt, Description("CreateMixDbData prompt for analyzing IoT health data")]
        public static ChatMessage CreateHeartBreathPrompt(
            [Description("Heart rate data")] string heartRate,
            [Description("Breath rate data")] string breathRate,
            [Description("Heart phase data")] string heartPhase,
            [Description("Breath phase data")] string breathPhase) =>
            new(ChatRole.User, $"""
                Please analyze the following health data from IoT device:
                - Heart Rate: {heartRate} bpm
                - Breath Rate: {breathRate} breaths / min
                - Heart Phase: {heartPhase}
                - Breath Phase: {breathPhase}

                Requirements for the analysis:
                1. Check if the values are within normal ranges:
                   - Heart rate: 60 - 100 bpm
                   - Breath rate: 12 - 20 breaths / min
                2. Analyze the relationship between heart rate and breath rate
                3. Check if heart and breath phases are synchronized
                4. Identify any potential health concerns
                5. Provide recommendations if values are abnormal
                6. Consider the context of the measurements
                7. Check for any patterns or trends
                8. Evaluate the quality of the data
                9. Suggest any necessary follow-up actions
                10. Provide a summary of the findings

                Please provide:
                1. A detailed analysis of the data
                2. Any health concerns identified
                3. Recommendations for next steps
                4. Suggestions for monitoring
                """);
    }
} 