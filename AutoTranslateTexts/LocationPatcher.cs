using System;
using System.Collections.Generic;
using System.Text;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public class LocationPatcher
    {
        public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, bool log)
        {
            var locationTranslations = new Dictionary<FormKey, LocationTranslation>();

            foreach (var location in state.LoadOrder.PriorityOrder.Location().WinningOverrides())
            {
                ProcessLocation(location, locationTranslations);
            }
            
            if (log)
            {
                PrintTranslations(locationTranslations);
            }
        }

        private static void ProcessLocation(ILocationGetter location,
            Dictionary<FormKey, LocationTranslation> locationTranslations)
        {
            var translation = new LocationTranslation();

            if (location.Name is { String: not null }) translation.Name = EncodingHelper.ConvertToUtf8(location.Name.String);

            locationTranslations.TryAdd(location.FormKey, translation);
        }

        private static void PrintTranslations(Dictionary<FormKey, LocationTranslation> locationTranslations)
        {
            var patcherName = "LocationPatcher";
            var logFilePath = Path.Combine(Environment.CurrentDirectory, $"WhitesLove-Patcher-AutoTranslateTexts-{patcherName}.log");
            
            using (var logWriter = new StreamWriter(logFilePath, false, Encoding.UTF8))
            {
                foreach (var entry in locationTranslations)
                {
                    logWriter.WriteLine($"FormKey: {entry.Key}");
                    logWriter.WriteLine($"Name: {entry.Value.Name}");
                    logWriter.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"Log file created at: {logFilePath}");
        }
    }

    public class LocationTranslation
    {
        public string Name { get; set; } = string.Empty;
    }
}