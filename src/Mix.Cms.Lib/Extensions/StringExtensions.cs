namespace Mix.Cms.Lib.Extensions
{
    public static class StringExtensions
    {
        public static string ToSourceSet(this string source)
        {
            string ext = source[source.IndexOf(".")..];
            string url = source[..source.IndexOf(".")];
            return @$"{url}_S{ext} 300w,{url}_M{ext} 600w,{url}_L{ext} 1000w,{url}_XL{ext} 2000w";
        }
    }
}
