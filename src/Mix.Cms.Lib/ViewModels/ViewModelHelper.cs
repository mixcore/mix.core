using Mix.Domain.Core.ViewModels;

namespace Mix.Cms.Lib.ViewModels
{
    public class ViewModelHelper
    {
        public static void HandleResult<T>(RepositoryResponse<T> result, ref RepositoryResponse<bool> output)
        {
            if (!result.IsSucceed)
            {
                output.IsSucceed = false;
                output.Exception = result.Exception;
                output.Errors = result.Errors;
            }
        }
    }
}