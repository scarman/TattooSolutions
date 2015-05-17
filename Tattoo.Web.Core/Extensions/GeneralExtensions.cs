namespace Tattoo.Web.Core.Extensions
{
    public static class GeneralExtensions
    {
        public static string Shorten(this string value, int size, bool includeDots = true)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length <= size)
                return value;

            return includeDots
                ? value.Substring(0, size - 3) + "..."
                : value.Substring(0, size);
        }
    }
}