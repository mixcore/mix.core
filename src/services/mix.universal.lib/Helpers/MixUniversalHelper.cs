using Mix.Heart.Helpers;
using Mix.Identity.Models.AccountViewModels;
using Mix.Universal.Lib.Dtos;
using Newtonsoft.Json.Linq;

namespace Mix.Universal.Lib.Helpers
{
    public class MixUniversalHelper
    {
        public static RegisterViewModel ParseRegisterDto(RegisterDto dto)
        {
            RegisterViewModel result = new();
            ReflectionHelper.Mapping(dto, result);
            result.Data = ReflectionHelper.ParseObject(dto.Information);
            return result;
        }
    }
}
