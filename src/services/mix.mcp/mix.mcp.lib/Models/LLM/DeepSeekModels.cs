using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Models.LLM
{
    public class DeepseekResponse
    {
        public List<DeepseekChoice>? Choices { get; set; }
    }

    public class DeepseekChoice
    {
        public DeepseekMessage? Message { get; set; }
    }

    public class DeepseekMessage
    {
        public string? Content { get; set; }
    }
}
