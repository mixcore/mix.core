using Microsoft.AspNetCore.Mvc;


namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static void CustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(
                options =>
                {
                    options.InvalidModelStateResponseFactory =
                        actionContext =>
                        {
                            return CustomErrorResponse(actionContext);
                        };
                }
            );
        }

        private static BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            List<string> errors = new();
            actionContext.ModelState
              .Where(modelError => modelError.Value.Errors.Count > 0)
              .ToList()
              .ForEach(modelError =>
              {
                  errors.AddRange(modelError.Value.Errors.Select(err => err.ErrorMessage).ToList());
              }
              );
            return new BadRequestObjectResult(errors);
        }
    }

}
