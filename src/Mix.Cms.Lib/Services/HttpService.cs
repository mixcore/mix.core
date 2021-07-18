using Mix.Cms.Lib.Constants;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Services
{
    public class HttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _sharedJsonSerializerOptions;
        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _sharedJsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };

        }

        public async Task<string> DownloadAsync(
            string downloadUrl, string downloadPath, string fileName, string extension,
            IProgress<int> progress, CancellationToken token)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (HttpResponseMessage response = client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();
                string fullPath = $"{downloadPath}/{fileName}{extension}";
                MixFileRepository.Instance.CreateDirectoryIfNotExist(downloadPath);
                using (Stream contentStream = await response.Content.ReadAsStreamAsync(), 
                    fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var totalReads = 0L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;
                    var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                    var canReportProgress = total != -1 && progress != null;
                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, read);

                            totalRead += read;
                            totalReads += 1;

                            if (canReportProgress)
                            {
                                progress.Report((int)Math.Round((totalRead * 1d) / (total * 1d) * 100, 0));
                            }
                        }
                    }
                    while (isMoreToRead);

                    return fullPath;
                }
            }
        }

        public Task<T> GetAsync<T>(
            string requestUrl,
            List<KeyValuePair<string, string>> queryParams = null,
            string bearerToken = null,
            List<KeyValuePair<string, string>> requestHeaders = null)
        {
            var urlQueryParamsPart = queryParams != null
                ? string.Join("&", queryParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"))
                : string.Empty;
            var requestUrlWithQueryParams = !string.IsNullOrEmpty(urlQueryParamsPart)
                ? (!requestUrl.Contains("?") ? $"{requestUrl}?{urlQueryParamsPart}" : $"{requestUrl}&{urlQueryParamsPart}")
                : requestUrl;
            return SendRequestAsync<T>(client => client.GetAsync(requestUrlWithQueryParams), bearerToken, requestHeaders);
        }

        public Task<T> PostAsync<T, T1>(string requestUrl, T1 content, string bearerToken = null, List<KeyValuePair<string, string>> requestHeaders = null) =>
            SendRequestAsync<T>(
                client => client.PostAsync(requestUrl, CreateHttpContent(content)), bearerToken, requestHeaders);

        private HttpContent CreateHttpContent<T>(T content) =>
            new StringContent(JsonSerializer.Serialize(content, _sharedJsonSerializerOptions), Encoding.UTF8, "application/json");

        private Task<T> SendRequestAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> sendRequestFn, string token = null, List<KeyValuePair<string, string>> requestHeaders = null) =>
            Task.Run(async () =>
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    requestHeaders?.ForEach(p => client.DefaultRequestHeaders.Add(p.Key, p.Value));

                    var response = await sendRequestFn(client);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(data, _sharedJsonSerializerOptions);
                }
            });
    }
}
