using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Mix.Lib.Models.ApiResponse;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Reflection;

namespace Mix.Lib.Filters
{
    /// <summary>
    /// Swagger operation filter that adds standard responses to all API endpoints
    /// </summary>
    public class SwaggerDefaultResponsesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Ensure we have a response dictionary
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }

            // Add common error responses for all API endpoints
            AddErrorResponse(operation, HttpStatusCode.InternalServerError, "Internal server error occurred");
            AddErrorResponse(operation, HttpStatusCode.BadRequest, "Invalid request");

            // Add auth-related responses if the endpoint requires authorization
            if (context.MethodInfo.DeclaringType.GetCustomAttributes<AuthorizeAttribute>(true).Any() ||
                context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any())
            {
                AddErrorResponse(operation, HttpStatusCode.Unauthorized, "Unauthorized access");
                AddErrorResponse(operation, HttpStatusCode.Forbidden, "Access forbidden");
            }

            // Add example responses when applicable
            AddResponseExamples(operation, context);
        }

        private void AddErrorResponse(OpenApiOperation operation, HttpStatusCode statusCode, string description)
        {
            string statusCodeString = ((int)statusCode).ToString();

            if (!operation.Responses.ContainsKey(statusCodeString))
            {
                var response = new OpenApiResponse
                {
                    Description = description,
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.Schema,
                                    Id = nameof(ApiErrorResponse)
                                }
                            }
                        }
                    }
                };

                operation.Responses.Add(statusCodeString, response);
            }
        }

        private void AddResponseExamples(OpenApiOperation operation, OperationFilterContext context)
        {
            // Add example responses for specific status codes
            foreach (var (key, response) in operation.Responses)
            {
                if (int.TryParse(key, out var statusCode))
                {
                    switch (statusCode)
                    {
                        case 400: // Bad Request
                            AddErrorExample(response, HttpStatusCode.BadRequest, "Validation failed",
                                "One or more validation errors occurred",
                                new Dictionary<string, string[]>
                                {
                                    ["name"] = new[] { "Name is required" },
                                    ["email"] = new[] { "Invalid email format" }
                                });
                            break;

                        case 401: // Unauthorized
                            AddErrorExample(response, HttpStatusCode.Unauthorized, "Unauthorized",
                                "Authentication credentials are missing or invalid");
                            break;

                        case 403: // Forbidden
                            AddErrorExample(response, HttpStatusCode.Forbidden, "Forbidden",
                                "You don't have permission to access this resource");
                            break;

                        case 404: // Not Found
                            AddErrorExample(response, HttpStatusCode.NotFound, "Resource not found",
                                "The requested resource could not be found");
                            break;

                        case 500: // Internal Server Error
                            AddErrorExample(response, HttpStatusCode.InternalServerError, "Internal server error",
                                "An unexpected error occurred");
                            break;
                    }
                }
            }
        }

        private void AddErrorExample(OpenApiResponse response, HttpStatusCode statusCode, string message, string details,
            Dictionary<string, string[]> errors = null)
        {
            if (response.Content.TryGetValue("application/json", out var mediaType))
            {
                var example = new OpenApiObject
                {
                    ["statusCode"] = new OpenApiInteger((int)statusCode),
                    ["message"] = new OpenApiString(message),
                    ["details"] = new OpenApiString(details),
                    ["timestamp"] = new OpenApiString(DateTime.UtcNow.ToString("o"))
                };

                // Add errors if provided
                if (errors != null)
                {
                    var errorsObj = new OpenApiObject();
                    foreach (var (key, values) in errors)
                    {
                        var valuesArray = new OpenApiArray();
                        foreach (var value in values)
                        {
                            valuesArray.Add(new OpenApiString(value));
                        }
                        errorsObj.Add(key, valuesArray);
                    }
                    example.Add("errors", errorsObj);
                }

                mediaType.Example = example;
            }
        }
    }
}