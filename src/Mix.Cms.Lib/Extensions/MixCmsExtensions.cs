using Mix.Cms.Lib.Helpers;

namespace Mix.Cms.Lib.Extensions
{
    public static class MixCmsExtensions
    {
        public static string ToMoney(this double value){
            return MixCmsHelper.FormatPrice(value);
        }
    }
}
