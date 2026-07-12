using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Strings;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    /// <summary>
    /// Describes a single translatable string field on a record: how to read it from a getter
    /// and how to write it onto a mutable override.
    /// </summary>
    public sealed class TranslatedField<TGetter, TSetter>
    {
        public TranslatedField(string name, Func<TGetter, ITranslatedStringGetter?> get, Action<TSetter, string> set)
        {
            Name = name;
            Get = get;
            Set = set;
        }

        public string Name { get; }
        public Func<TGetter, ITranslatedStringGetter?> Get { get; }
        public Action<TSetter, string> Set { get; }
    }

    /// <summary>
    /// Shared per-record override engine. For each winning record it scans every override of the
    /// same FormKey across the load order, and when a field on the winner is NOT in the target
    /// language but some override provides that field in the target language, it copies the
    /// target-language text onto the winner in the patch mod.
    /// </summary>
    public static class TranslationEngine
    {
        public static void Process<TGetter, TSetter>(
            IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
            string moduleName,
            IEnumerable<TGetter> winners,
            Func<FormKey, IEnumerable<TGetter>> resolveAll,
            Func<TGetter, TSetter> addOverride,
            IReadOnlyList<TranslatedField<TGetter, TSetter>> fields,
            TargetLanguage target,
            bool log)
            where TGetter : IMajorRecordGetter
        {
            var changes = new List<string>();

            foreach (var winner in winners)
            {
                var overrides = resolveAll(winner.FormKey).ToList();
                TSetter? patched = default;
                var created = false;

                foreach (var field in fields)
                {
                    var current = field.Get(winner)?.String;
                    var translated = FindTranslation(overrides, field.Get, current, target);
                    if (translated == null)
                    {
                        continue;
                    }

                    if (!created)
                    {
                        patched = addOverride(winner);
                        created = true;
                    }

                    field.Set(patched!, translated);
                    changes.Add($"{winner.FormKey} [{field.Name}] -> {translated}");
                }
            }

            if (log)
            {
                LogWriter.Write(moduleName, changes);
            }
        }

        /// <summary>
        /// Finds the target-language text for a field. Returns null when the current text is already
        /// in the target language, or when no override provides a target-language version.
        /// </summary>
        public static string? FindTranslation<TGetter>(
            IEnumerable<TGetter> overrides,
            Func<TGetter, ITranslatedStringGetter?> get,
            string? current,
            TargetLanguage target)
        {
            if (LanguageDetector.IsTarget(current, target))
            {
                return null;
            }

            foreach (var candidate in overrides)
            {
                var text = get(candidate)?.String;
                if (string.IsNullOrWhiteSpace(text) || text == current)
                {
                    continue;
                }

                if (LanguageDetector.IsTarget(text, target))
                {
                    return text;
                }
            }

            return null;
        }
    }

    /// <summary>Writes a per-module change log next to the patcher output.</summary>
    public static class LogWriter
    {
        public static void Write(string moduleName, IReadOnlyList<string> changes)
        {
            var logFilePath = Path.Combine(
                Environment.CurrentDirectory,
                $"WhitesLove-Patcher-AutoTranslateTexts-{moduleName}.log");

            using (var writer = new StreamWriter(logFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine($"Module: {moduleName}");
                writer.WriteLine($"Fields changed: {changes.Count}");
                writer.WriteLine(new string('-', 40));
                foreach (var change in changes)
                {
                    writer.WriteLine(change);
                }
            }

            Console.WriteLine($"[{moduleName}] {changes.Count} field(s) patched. Log: {logFilePath}");
        }
    }
}
