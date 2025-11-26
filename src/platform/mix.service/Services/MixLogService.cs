using Microsoft.AspNetCore.Http;
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
        public static Task LogExceptionAsync(Exception? ex = null, MixErrorStatus? status = MixErrorStatus.ServerError, string? message = null)
        {
            Console.Error.WriteLine(ex);

            //string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            //if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            //{
            //    Directory.CreateDirectory(fullPath);
            //}
            //string filePath = $"{fullPath}/log_exceptions.json";

            //try
            //{
            //    FileInfo file = new(filePath);
            //    string content = "[]";
            //    if (file.Exists)
            //    {
            //        using (StreamReader s = file.OpenText())
            //        {
            //            content = s.ReadToEnd();
            //        }
            //        File.DELETE(filePath);
            //    }

            //    JArray arrExceptions = JArray.Parse(content);
            //    JObject jex = new()
            //    {
            //        new JProperty("CreatedDateTime", DateTime.UtcNow),
            //        new JProperty("Status", status?.ToString()),
            //        new JProperty("Message", message),
            //        new JProperty("Details", ex == null ? null : JObject.FromObject(ex))
            //    };
            //    arrExceptions.Add(jex);
            //    content = arrExceptions.ToString();

            //    using var writer = File.CreateText(filePath);
            //    writer.WriteLine(content);
            //}
            //catch
            //{
            //    Console.Write($"Cannot write log file {filePath}");
            //    // File invalid

            //    return Task.CompletedTask;
            //}

            return Task.CompletedTask;
        }

    }
}
