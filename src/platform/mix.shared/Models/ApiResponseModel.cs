using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Models
{
    public class ApiResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
        public string[]? Errors { get; set; }
        public Exception? Exception { get; set; }

        public ApiResponseModel(bool isSuccess, T? result = default, params string[] messages)
        {
            IsSuccess = isSuccess;
            Result = result;
            Errors = messages;
        }
    }
}
