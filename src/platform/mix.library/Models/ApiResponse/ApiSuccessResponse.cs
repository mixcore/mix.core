using System.Net;
using System.Text.Json.Serialization;

namespace Mix.Lib.Models.ApiResponse
{
    /// <summary>
    /// Standardized success response for API endpoints
    /// </summary>
    /// <typeparam name="T">Type of the data being returned</typeparam>
    public class ApiSuccessResponse<T>
    {
        /// <summary>
        /// Success status code
        /// </summary>
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;

        /// <summary>
        /// Success message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = "Operation completed successfully";

        /// <summary>
        /// Timestamp when the response was created
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The response data
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; }

        /// <summary>
        /// Creates a new API success response
        /// </summary>
        /// <param name="data">Response data</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static ApiSuccessResponse<T> Create(T data, HttpStatusCode statusCode = HttpStatusCode.OK, string message = null)
        {
            return new ApiSuccessResponse<T>
            {
                StatusCode = (int)statusCode,
                Message = message ?? "Operation completed successfully",
                Data = data
            };
        }

        /// <summary>
        /// Creates an OK response
        /// </summary>
        /// <param name="data">Response data</param>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static ApiSuccessResponse<T> Ok(T data, string message = "Operation completed successfully")
        {
            return Create(data, HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Creates a Created response
        /// </summary>
        /// <param name="data">Created entity data</param>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static ApiSuccessResponse<T> Created(T data, string message = "Resource created successfully")
        {
            return Create(data, HttpStatusCode.Created, message);
        }

        /// <summary>
        /// Creates a No Content response
        /// </summary>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static ApiSuccessResponse<T> NoContent(string message = "Operation completed successfully with no content")
        {
            return Create(default, HttpStatusCode.NoContent, message);
        }
    }

    /// <summary>
    /// Non-generic success response for endpoints that don't return data
    /// </summary>
    public class ApiSuccessResponse : ApiSuccessResponse<object>
    {
        /// <summary>
        /// Creates a success response with no data
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static new ApiSuccessResponse Create(HttpStatusCode statusCode = HttpStatusCode.OK, string message = null)
        {
            return new ApiSuccessResponse
            {
                StatusCode = (int)statusCode,
                Message = message ?? "Operation completed successfully",
                Data = null
            };
        }

        /// <summary>
        /// Creates an OK response with no data
        /// </summary>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static new ApiSuccessResponse Ok(string message = "Operation completed successfully")
        {
            return (ApiSuccessResponse)Create(HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Creates a No Content response
        /// </summary>
        /// <param name="message">Success message</param>
        /// <returns>The API success response</returns>
        public static new ApiSuccessResponse NoContent(string message = "Operation completed successfully with no content")
        {
            return (ApiSuccessResponse)Create(HttpStatusCode.NoContent, message);
        }
    }
}