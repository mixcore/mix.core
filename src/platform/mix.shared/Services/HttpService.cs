using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Mix.Shared.Services
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
            };

        }
        public async Task<JObject?> SendHttpRequestModel(
           HttpRequestModel request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string method = request.Method?.ToUpper();
            switch (method)
            {
                case "GET":
                    return await GetAsync<JObject>(request.RequestUrl);
                case "POST":
                    return await PostAsync<JObject, JObject>(request.RequestUrl, request.Body);
            }
            return default;
        }

        public async Task<string> DownloadAsync(
            string downloadUrl, string folder, string fileName, string extension,
            IProgress<int> progress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var client = _httpClientFactory.CreateClient())
            using (HttpResponseMessage response = client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken).Result)
            {
                response.EnsureSuccessStatusCode();
                //string folder = $"{MixFolders.WebRootPath}/{downloadPath}";
                string fullPath = $"{folder}/{fileName}{extension}";
                MixFileHelper.CreateFolderIfNotExist(folder);
                using (Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken),
                    fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var totalReads = 0L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;
                    var total = response.Content.Headers.ContentLength ?? -1L;
                    var canReportProgress = total != -1 && progress != null;
                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, read, cancellationToken);

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
            Dictionary<string, string> queryParams = null,
            string bearerToken = null,
            List<KeyValuePair<string, string>> requestHeaders = null)
        {
            var urlQueryParamsPart = queryParams != null
                ? string.Join("&", queryParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"))
                : string.Empty;
            var requestUrlWithQueryParams = !string.IsNullOrEmpty(urlQueryParamsPart)
                ? (!requestUrl.Contains('?') ? $"{requestUrl}?{urlQueryParamsPart}" : $"{requestUrl}&{urlQueryParamsPart}")
                : requestUrl;
            return SendRequestAsync<T>(client => client.GetAsync(requestUrlWithQueryParams), bearerToken, requestHeaders);
        }

        public Task<string> GetStringAsync(
            string requestUrl,
            List<KeyValuePair<string, string>> queryParams = null,
            string bearerToken = null,
            List<KeyValuePair<string, string>> requestHeaders = null)
        {
            var urlQueryParamsPart = queryParams != null
                ? string.Join("&", queryParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"))
                : string.Empty;
            var requestUrlWithQueryParams = !string.IsNullOrEmpty(urlQueryParamsPart)
                ? (!requestUrl.Contains('?') ? $"{requestUrl}?{urlQueryParamsPart}" : $"{requestUrl}&{urlQueryParamsPart}")
                : requestUrl;
            return GetResponseStringAsync(client => client.GetAsync(requestUrlWithQueryParams), bearerToken, requestHeaders);
        }

        public Task DeleteAsync(
            string requestUrl,
            List<KeyValuePair<string, string>> queryParams = null,
            string bearerToken = null,
            List<KeyValuePair<string, string>> requestHeaders = null)
        {
            var urlQueryParamsPart = queryParams != null
                ? string.Join("&", queryParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}"))
                : string.Empty;
            var requestUrlWithQueryParams = !string.IsNullOrEmpty(urlQueryParamsPart)
                ? (!requestUrl.Contains('?') ? $"{requestUrl}?{urlQueryParamsPart}" : $"{requestUrl}&{urlQueryParamsPart}")
                : requestUrl;
            return SendRequestAsync(client => client.DeleteAsync(requestUrlWithQueryParams), bearerToken, requestHeaders);
        }

        public Task<T> PostAsync<T, T1>(string requestUrl, T1 body,
                string bearerToken = null,
                List<KeyValuePair<string, string>> requestHeaders = null,
                string contentType = "application/json"
            )
        {
            var content = CreateHttpContent(body, contentType);
            return SendRequestAsync<T>(
                client => client.PostAsync(requestUrl, content), bearerToken, requestHeaders);
        }
        #region Privates
                
        private HttpContent CreateHttpContent<T>(T content, string contentType)
        {
            switch (contentType)
            {
                case "multipart/form-data":
                    try
                    {
                        var formfile = content as IFormFile;
                        var multipartFormContent = new MultipartFormDataContent();
                        //Load the file and set the file's Content-Type header
                        var fileStreamContent = new StreamContent(formfile.OpenReadStream());
                        multipartFormContent.Add(fileStreamContent, name: "file", fileName: formfile.FileName);
                        return multipartFormContent;
                    }
                    catch (Exception ex)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, ex);
                    }
                case "application/x-www-form-urlencoded":
                    var formData = content.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(content, null));
                    return new FormUrlEncodedContent(formData);
                default:
                    return new StringContent(JsonSerializer.Serialize(content, _sharedJsonSerializerOptions), Encoding.UTF8, contentType);
            }


        }
        private async Task<T> SendRequestAsync<T>(
                Func<HttpClient, Task<HttpResponseMessage>> sendRequestFn,
                string token = null,
                List<KeyValuePair<string, string>> requestHeaders = null)

        {
            var data = await GetResponseStringAsync(sendRequestFn, token, requestHeaders);
            if (data.IsJsonString())
            {
                var obj = JObject.Parse(data);
                return obj.ToObject<T>();
            }
            else
            {
                return new JObject(new JProperty("data", data)).ToObject<T>();
            }
        }

        private async Task<string> GetResponseStringAsync(
                Func<HttpClient, Task<HttpResponseMessage>> sendRequestFn,
                string token = null,
                List<KeyValuePair<string, string>> requestHeaders = null)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                if (token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                requestHeaders?.ForEach(p => client.DefaultRequestHeaders.Add(p.Key, p.Value));

                var response = await sendRequestFn(client);
                var data = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new MixException(data);
                }
                response.EnsureSuccessStatusCode();
                return data;
            }
        }

        private Task SendRequestAsync(
                Func<HttpClient, Task<HttpResponseMessage>> sendRequestFn,
                string token = null,
                List<KeyValuePair<string, string>> requestHeaders = null) =>
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
                    var data = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new MixException(data);
                    }
                    response.EnsureSuccessStatusCode();
                    return Task.CompletedTask;
                }
            });

        #endregion
    }
}
