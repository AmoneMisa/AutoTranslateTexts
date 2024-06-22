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

                if (log)
                {
                    PrintTranslations(locationTranslations);
                }
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
            foreach (var entry in locationTranslations)
            {
                Console.WriteLine($"FormKey: {entry.Key}");
                Console.WriteLine($"Name: {entry.Value.Name}");
                Console.WriteLine(new string('-', 40));
            }
        }
    }

    public class LocationTranslation
    {
        public string Name { get; set; } = string.Empty;
    }
}