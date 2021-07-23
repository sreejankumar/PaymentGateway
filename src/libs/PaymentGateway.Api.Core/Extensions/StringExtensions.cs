
namespace PaymentGateway.Api.Core.Extensions
{
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }


        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            return s[0].ToString().ToLower() + s.Substring(1, s.Length - 1);
        }
    }
}
