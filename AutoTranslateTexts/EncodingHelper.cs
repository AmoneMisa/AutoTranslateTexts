using System;
using System.Text;

namespace AutoTranslateTexts
{
    public static class EncodingHelper
    {
        public static string ConvertToUtf8(string text)
        {
            if (IsUtf8(text))
            {
                return text;
            }

            var bytes = Encoding.Default.GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }

        private static bool IsUtf8(string text)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                var decoded = Encoding.UTF8.GetString(bytes);
                return decoded.Equals(text);
            }
            catch
            {
                return false;
            }
        }
    }
}