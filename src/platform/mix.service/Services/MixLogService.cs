using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Shared.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using Quartz;

namespace Mix.Service.Services
{
    public class MixLogService
    {
        private static readonly ILogStreamHubClientService _logStreamHub;
        static MixLogService()
        {
            MixEndpointService enpointSrv = new();
            _logStreamHub = new LogStreamHubClientService(enpointSrv);
        }

        public static async Task LogExceptionAsync(Exception? ex = null, MixErrorStatus? status = null, string? message = null)
        {
            Console.Error.WriteLine(ex);

            string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                FileInfo file = new(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new()
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Status", status?.ToString()),
                    new JProperty("Message", message),
                    new JProperty("Details", ex == null ? null : JObject.FromObject(ex))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using var writer = File.CreateText(filePath);
                writer.WriteLine(content);
            }
            catch
            {
                Console.Write($"Cannot write log file {filePath}");
                // File invalid
            }
            finally
            {
                await SendMessage(message, ex: ex, msgType: MessageType.Error);
            }
        }
        public static async Task LogErrorAsync(string message, object data)
        {
            string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                FileInfo file = new(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new()
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Message", message),
                    new JProperty("Data", data == null ? null : JObject.FromObject(data))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using var writer = File.CreateText(filePath);
                writer.WriteLine(content);
            }
            catch
            {
                Console.Write($"Cannot write log file {filePath}");
                // File invalid
            }
            finally
            {
                await SendMessage(message, msgType: MessageType.Error);
            }
        }

        public static async Task LogMessageAsync(string message, object? data = default, MessageType msgType = MessageType.Info)
        {
            await SendMessage(message, data, msgType: msgType);
        }

        private static async Task SendMessage(string? message, object? data = default, Exception? ex = null, MessageType msgType = MessageType.Info)
        {
            var obj = ReflectionHelper.ParseObject(data ?? ex);
            if (obj != null)
            {
                SignalRMessageModel msg = new()
                {
                    Action = MessageAction.NewMessage,
                    Type = msgType,
                    Title = message,
                    From = new("Log Stream Service"),
                    Data = obj.ToString(Newtonsoft.Json.Formatting.None),
                    Message = ex == null ? message : ex!.Message
                };
                if (GlobalConfigService.Instance.IsLogStream)
                {
                    await _logStreamHub.SendMessageAsync(msg);
                }
            }
        }

    }
}
