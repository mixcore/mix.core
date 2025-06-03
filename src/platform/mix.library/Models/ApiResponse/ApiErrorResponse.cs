using System.Net;
using System.Text.Json.Serialization;

namespace Mix.Lib.Models.ApiResponse
{
    /// <summary>
    /// Standardized error response for API endpoints
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Error status code
        /// </summary>
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Detailed error information
        /// </summary>
        [JsonPropertyName("details")]
        public string Details { get; set; }

        /// <summary>
        /// Timestamp when the error occurred
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional error data
        /// </summary>
        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; }

        /// <summary>
        /// Creates a new API error response
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Detailed error information</param>
        /// <param name="errors">Additional validation errors</param>
        /// <returns>The API error response</returns>
        public static ApiErrorResponse Create(
            HttpStatusCode statusCode,
            string message,
            string details = null,
            Dictionary<string, string[]> errors = null)
        {
            return new ApiErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message,
                Details = details,
                Errors = errors
            };
        }

        /// <summary>
        /// Creates a not found error response
        /// </summary>
        /// <param name="resourceName">Name of the resource that wasn't found</param>
        /// <param name="resourceId">ID of the resource that wasn't found</param>
        /// <returns>The API error response</returns>
        public static ApiErrorResponse NotFound(string resourceName, object resourceId)
        {
            return Create(
                HttpStatusCode.NotFound,
                $"{resourceName} not found",
                $"The requested {resourceName} with ID {resourceId} could not be found."
            );
        }

        /// <summary>
        /// Creates a bad request error response
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="errors">Validation errors</param>
        /// <returns>The API error response</returns>
        public static ApiErrorResponse BadRequest(string message, Dictionary<string, string[]> errors = null)
        {
            return Create(
                HttpStatusCode.BadRequest,
                message,
                errors: errors
            );
        }

        /// <summary>
        /// Creates an unauthorized error response
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>The API error response</returns>
        public static ApiErrorResponse Unauthorized(string message = "Unauthorized access")
        {
            return Create(
                HttpStatusCode.Unauthorized,
                message
            );
        }

        /// <summary>
        /// Creates a forbidden error response
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>The API error response</returns>
        public static ApiErrorResponse Forbidden(string message = "Access forbidden")
        {
            return Create(
                HttpStatusCode.Forbidden,
                message
            );
        }
    }
}