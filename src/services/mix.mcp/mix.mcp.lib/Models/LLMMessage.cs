using Mix.MCP.Lib.Services.LLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Models
{
    /// <summary>
    /// Represents a message in the conversation history
    /// </summary>
    public class LLMMessage
    {
        public string DeviceId { get; set; }
        public string SessionId { get; set; }
        public LLMServiceType ServiceType { get; set; } = LLMServiceType.DeepSeek;
        public LLMMessageContent Data { get; set; } = new LLMMessageContent();
    }

    public class LLMMessageContent
    {
        public string Role { get; set; } = "user";
        public string Content { get; set; } = string.Empty;
    }
}
