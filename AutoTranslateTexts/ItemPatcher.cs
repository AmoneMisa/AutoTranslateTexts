using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Strings;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public class ItemPatcher
    {
        public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, bool log)
        {
            var itemTranslations = new Dictionary<FormKey, ItemTranslation>();
            var itemTypes = new[] { "Armor", "Weapon", "Potion", "Scroll", "Ingredient", "MiscItem", "Key", "SoulGem" };

            foreach (var itemType in itemTypes)
            {
                var items = state.LoadOrder.PriorityOrder;

                var winningOverridesMethod = items.GetType().GetMethod("WinningOverrides");
                if (winningOverridesMethod == null) continue;

                if (winningOverridesMethod.Invoke(items, null) is not IEnumerable<object> winningItems) continue;

                foreach (var item in winningItems)
                {
                    ProcessAndPrintItem(item, itemTranslations);
                }
            }
            
            if (log)
            {
                PrintTranslations(itemTranslations);
            }
        }

        private static void ProcessAndPrintItem(object item, Dictionary<FormKey, ItemTranslation> itemTranslations)
        {
            var itemType = item.GetType();
            var formKeyProperty = itemType.GetProperty("FormKey");
            if (formKeyProperty == null) return;

            var formKey = (FormKey?)formKeyProperty.GetValue(item);
            if (formKey == null) return;

            var translation = new ItemTranslation();

            var nameProperty = itemType.GetProperty("Name");
            if (nameProperty != null && nameProperty.GetValue(item) is ITranslatedStringGetter
                {
                    String: not null
                } nameValue) translation.Name = EncodingHelper.ConvertToUtf8(nameValue.String);

            var descriptionProperty = itemType.GetProperty("Description");
            if (descriptionProperty != null && descriptionProperty.GetValue(item) is ITranslatedStringGetter
                {
                    String: not null
                } descriptionValue)
                translation.Description = EncodingHelper.ConvertToUtf8(descriptionValue.String);

            itemTranslations.TryAdd(formKey.Value, translation);
        }

        private static void PrintTranslations(Dictionary<FormKey, ItemTranslation> itemTranslations)
        {
            var patcherName = "ItemPatcher";
            var logFilePath = Path.Combine(Environment.CurrentDirectory, $"WhitesLove-Patcher-AutoTranslateTexts-{patcherName}.log");
            
            using (var logWriter = new StreamWriter(logFilePath, false, Encoding.UTF8))
            {
                foreach (var entry in itemTranslations)
                {
                    logWriter.WriteLine($"FormKey: {entry.Key}");
                    logWriter.WriteLine($"Name: {entry.Value.Name}");
                    logWriter.WriteLine($"Description: {entry.Value.Description}");
                    logWriter.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"Log file created at: {logFilePath}");
        }
    }

    public class ItemTranslation
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}