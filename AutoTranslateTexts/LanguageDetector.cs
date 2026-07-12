using System;
using System.Linq;

namespace AutoTranslateTexts
{
    /// <summary>
    /// Languages the patcher can target. Detection is script-based, so only languages
    /// with a distinct script can be told apart. Every Latin-script language
    /// (English, French, German, Italian, Spanish, Polish, ...) is reported as <see cref="Latin"/>.
    /// </summary>
    public enum TargetLanguage
    {
        Latin,
        Russian,
        Chinese,
        Japanese,
        Korean,
        Arabic,
        Greek
    }

    /// <summary>
    /// Detects the dominant script of a string and maps it to a <see cref="TargetLanguage"/>.
    /// </summary>
    public static class LanguageDetector
    {
        public static bool IsTarget(string? text, TargetLanguage target)
        {
            var detected = Detect(text);
            return detected.HasValue && detected.Value == target;
        }

        /// <summary>
        /// Returns the dominant language of <paramref name="text"/>, or null when the string
        /// contains no letters we can classify.
        /// </summary>
        public static TargetLanguage? Detect(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            int latin = 0, cyrillic = 0, han = 0, kana = 0, hangul = 0, arabic = 0, greek = 0;

            foreach (var ch in text)
            {
                if (char.IsWhiteSpace(ch) || char.IsDigit(ch) || char.IsPunctuation(ch) ||
                    char.IsSymbol(ch) || char.IsControl(ch))
                {
                    continue;
                }

                int c = ch;

                if (c >= 0x0400 && c <= 0x04FF) cyrillic++;
                else if ((c >= 0x0041 && c <= 0x005A) || (c >= 0x0061 && c <= 0x007A) || (c >= 0x00C0 && c <= 0x024F)) latin++;
                else if (c >= 0x4E00 && c <= 0x9FFF) han++;
                else if ((c >= 0x3040 && c <= 0x309F) || (c >= 0x30A0 && c <= 0x30FF)) kana++;
                else if (c >= 0xAC00 && c <= 0xD7A3) hangul++;
                else if ((c >= 0x0600 && c <= 0x06FF) || (c >= 0x0750 && c <= 0x077F)) arabic++;
                else if (c >= 0x0370 && c <= 0x03FF) greek++;
            }

            // Kana or Hangul are unambiguous markers of Japanese / Korean.
            if (kana > 0) return TargetLanguage.Japanese;
            if (hangul > 0) return TargetLanguage.Korean;

            var scores = new (TargetLanguage lang, int count)[]
            {
                (TargetLanguage.Latin, latin),
                (TargetLanguage.Russian, cyrillic),
                (TargetLanguage.Chinese, han),
                (TargetLanguage.Arabic, arabic),
                (TargetLanguage.Greek, greek)
            };

            var best = scores.OrderByDescending(s => s.count).First();
            return best.count == 0 ? null : best.lang;
        }
    }
}
