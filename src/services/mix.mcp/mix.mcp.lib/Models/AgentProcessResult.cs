
namespace Mix.MCP.Lib.Models
{
    public class AgentProcessResult
    {
        public bool IsSuccess { get; set; }
        public string Response { get; set; } = string.Empty;
        public string Result => IsSuccess ? "Success" : "Failure";
        public AgentProcessResult(bool isSuccess, string response)
        {
            IsSuccess = isSuccess;
            Response = response;
        }
    }
}