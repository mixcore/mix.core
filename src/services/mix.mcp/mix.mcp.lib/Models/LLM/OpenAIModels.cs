using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Models.LLM
{
    public class OpenAIResponse
    {
        public List<OpenAIChoice>? Choices { get; set; }
    }

    public class OpenAIChoice
    {
        public OpenAIMessage? Message { get; set; }
    }

    public class OpenAIMessage
    {
        public string? Content { get; set; }
    }
}
