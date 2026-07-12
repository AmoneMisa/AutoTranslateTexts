using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    /// <summary>
    /// Translates dialogue. Unlike the other modules, dialogue text lives in nested response
    /// lists (INFO records inside a DIAL topic), so it cannot use the flat TranslationEngine.
    /// It patches the topic prompt (DialogTopic.Name) and every individual response line.
    /// </summary>
    public sealed class DialogueModule : IPatcherModule
    {
        public string Name => "DialoguePatcher";

        public ModuleSetting GetSettings(Settings settings) => settings.Dialogue;

        public void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log)
        {
            var changes = new List<string>();

            foreach (var topic in state.LoadOrder.PriorityOrder.DialogTopic().WinningOverrides())
            {
                var topicOverrides = state.LinkCache.ResolveAll<IDialogTopicGetter>(topic.FormKey).ToList();
                DialogTopic? patched = null;

                // Topic prompt text.
                var translatedName = TranslationEngine.FindTranslation(
                    topicOverrides, t => t.Name, topic.Name?.String, target);
                if (translatedName != null)
                {
                    patched = state.PatchMod.DialogTopics.GetOrAddAsOverride(topic);
                    patched.Name = translatedName;
                    changes.Add($"{topic.FormKey} [Name] -> {translatedName}");
                }

                // Individual response lines, matched by response index within each INFO record.
                for (var infoIndex = 0; infoIndex < topic.Responses.Count; infoIndex++)
                {
                    var info = topic.Responses[infoIndex];
                    var infoOverrides = state.LinkCache.ResolveAll<IDialogResponsesGetter>(info.FormKey).ToList();

                    for (var responseIndex = 0; responseIndex < info.Responses.Count; responseIndex++)
                    {
                        var current = info.Responses[responseIndex].Text?.String;
                        if (LanguageDetector.IsTarget(current, target))
                        {
                            continue;
                        }

                        var translated = FindResponseTranslation(infoOverrides, responseIndex, current, target);
                        if (translated == null)
                        {
                            continue;
                        }

                        patched ??= state.PatchMod.DialogTopics.GetOrAddAsOverride(topic);
                        patched.Responses[infoIndex].Responses[responseIndex].Text = translated;
                        changes.Add($"{info.FormKey} [Response {responseIndex}] -> {translated}");
                    }
                }
            }

            if (log)
            {
                LogWriter.Write(Name, changes);
            }
        }

        private static string? FindResponseTranslation(
            IEnumerable<IDialogResponsesGetter> infoOverrides,
            int responseIndex,
            string? current,
            TargetLanguage target)
        {
            foreach (var info in infoOverrides)
            {
                if (responseIndex >= info.Responses.Count)
                {
                    continue;
                }

                var text = info.Responses[responseIndex].Text?.String;
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
}
