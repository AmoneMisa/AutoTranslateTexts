﻿using System;
using System.Collections.Generic;
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
        public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            var bookTranslations = new Dictionary<FormKey, BookTranslation>();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            foreach (var book in state.LoadOrder.PriorityOrder.Book().WinningOverrides())
            {
                ProcessBook(book, bookTranslations);
            }
            
            PrintTranslations(bookTranslations);
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
            foreach (var entry in bookTranslations)
            {
                Console.WriteLine($"FormKey: {entry.Key}");
                Console.WriteLine($"Title: {entry.Value.Title}");
                Console.WriteLine($"Description: {entry.Value.Description}");
                Console.WriteLine($"Content: {entry.Value.Content}");
                Console.WriteLine($"Language: {entry.Value.Language}");
                Console.WriteLine(new string('-', 40));
            }
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