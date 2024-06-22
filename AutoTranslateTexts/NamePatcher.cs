using System;
using System.Collections.Generic;
using System.Text;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public class NamePatcher
    {
        public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, bool log)
        {
            var nameTranslations = new Dictionary<FormKey, NameTranslation>();
            
            foreach (var npc in state.LoadOrder.PriorityOrder.Npc().WinningOverrides())
            {
                ProcessName(npc, nameTranslations);
            }
            
            if (log)
            {
                PrintTranslations(nameTranslations);
            }
        }

        private static void ProcessName(INpcGetter npc, Dictionary<FormKey, NameTranslation> nameTranslations)
        {
            var translation = new NameTranslation();

            if (npc.Name is  { String: not null })
            {
                translation.Name = EncodingHelper.ConvertToUtf8(npc.Name.String);
            }

            if (npc.ShortName is { String: not null })
            {
                translation.ShortName = EncodingHelper.ConvertToUtf8(npc.ShortName.String);
            }
            
            nameTranslations.TryAdd(npc.FormKey, translation);
        }

        private static void PrintTranslations(Dictionary<FormKey, NameTranslation> nameTranslations)
        {
            var patcherName = "NamePatcher";
            var logFilePath = Path.Combine(Environment.CurrentDirectory, $"WhitesLove-Patcher-AutoTranslateTexts-{patcherName}.log");
            
            using (var logWriter = new StreamWriter(logFilePath, false, Encoding.UTF8))
            {
                foreach (var entry in nameTranslations)
                {
                    logWriter.WriteLine($"FormKey: {entry.Key}");
                    logWriter.WriteLine($"Name: {entry.Value.Name}");
                    logWriter.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"Log file created at: {logFilePath}");
        }
    }

    public class NameTranslation
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}