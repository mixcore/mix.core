using Mix.Heart.Models;
using System;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {
        public static void HandleException<TResult>(RepositoryResponse<TResult> result, Exception ex)

        {
            result.IsSucceed = false;
            result.Exception = ex;
            result.Errors.Add(ex.Message);
            LogException(ex);
            result.Errors.Add(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }

        private static void LogException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
