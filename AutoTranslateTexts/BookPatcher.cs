using System;
using System.Collections.Generic;
using System.Text;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Strings;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public static class BookPatcher
    {
        public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, bool log)
        {
            var bookTranslations = new Dictionary<FormKey, BookTranslation>();

            foreach (var book in state.LoadOrder.PriorityOrder.Book().WinningOverrides())
            {
                ProcessBook(book, bookTranslations);
            }

            if (log)
            {
                PrintTranslations(bookTranslations);
            }
        }

        private static void ProcessBook(IBookGetter book, Dictionary<FormKey, BookTranslation> bookTranslations)
        {
            if (bookTranslations.TryGetValue(book.FormKey, out var translation))
            {
                UpdateTranslation(book, translation);
            }
            else
            {
                bookTranslations[book.FormKey] = CreateTranslation(book);
            }
        }

        private static BookTranslation CreateTranslation(IBookGetter book)
        {
            var translation = new BookTranslation
            {
                Title = book.Name?.String ?? string.Empty,
                Description = book.Description?.String ?? string.Empty,
                Content = book.BookText?.String ?? string.Empty,
                Language = GetLanguage(book.BookText)
            };

            if (translation.Language != "Russian")
            {
                return translation;
            }

            translation.Title = EncodingHelper.ConvertToUtf8(translation.Title);
            translation.Description = EncodingHelper.ConvertToUtf8(translation.Description);
            translation.Content = EncodingHelper.ConvertToUtf8(translation.Content);

            return translation;
        }

        private static void UpdateTranslation(IBookGetter book, BookTranslation translation)
        {
            var language = GetLanguage(book.BookText);
            if (translation.Language != "Russian" && language == "Russian")
            {
                translation.Title = EncodingHelper.ConvertToUtf8(book.Name?.String ?? string.Empty);
                translation.Description = EncodingHelper.ConvertToUtf8(book.Description?.String ?? string.Empty);
                translation.Content = EncodingHelper.ConvertToUtf8(book.BookText?.String ?? string.Empty);
                translation.Language = "Russian";
            }
            else if (translation.Language == "Russian" && language == "Russian")
            {
                translation.Title = EncodingHelper.ConvertToUtf8(book.Name?.String ?? string.Empty);
                translation.Description = EncodingHelper.ConvertToUtf8(book.Description?.String ?? string.Empty);
                translation.Content = EncodingHelper.ConvertToUtf8(book.BookText?.String ?? string.Empty);
            }
        }

        private static string GetLanguage(ITranslatedStringGetter? translatedString)
        {
            if (translatedString == null || string.IsNullOrWhiteSpace(translatedString.String))
                return "Unknown";

            var content = translatedString.String;

            return IsRussian(content) ? "Russian" : "Other";
        }

        private static bool IsRussian(string content)
        {
            return content.Any(ch => ch is >= 'А' and <= 'я' or 'ё' or 'Ё');
        }
        
        private static void PrintTranslations(Dictionary<FormKey, BookTranslation> bookTranslations)
        {
            var patcherName = "ItemPatcher";
            var logFilePath = Path.Combine(Environment.CurrentDirectory, $"WhitesLove-Patcher-AutoTranslateTexts-{patcherName}.log");
            
            using (var logWriter = new StreamWriter(logFilePath, false, Encoding.UTF8))
            {
                foreach (var entry in bookTranslations)
                {
                    logWriter.WriteLine($"FormKey: {entry.Key}");
                    logWriter.WriteLine($"Title: {entry.Value.Title}");
                    logWriter.WriteLine($"Description: {entry.Value.Description}");
                    logWriter.WriteLine($"Content: {entry.Value.Content}");
                    logWriter.WriteLine($"Language: {entry.Value.Language}");
                    logWriter.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"Log file created at: {logFilePath}");
        }
    }

    public class BookTranslation
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Language { get; set; } = "Unknown";
    }
}