using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public sealed class ItemModule : IPatcherModule
    {
        public string Name => "ItemPatcher";

        public ModuleSetting GetSettings(Settings settings) => settings.Items;

        public void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log)
        {
            TranslationEngine.Process<IArmorGetter, Armor>(
                state, Name,
                state.LoadOrder.PriorityOrder.Armor().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IArmorGetter>(fk),
                w => state.PatchMod.Armors.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IArmorGetter, Armor>("Name", a => a.Name, (a, v) => a.Name = v),
                    new TranslatedField<IArmorGetter, Armor>("Description", a => a.Description, (a, v) => a.Description = v)
                },
                target, log);

            TranslationEngine.Process<IWeaponGetter, Weapon>(
                state, Name,
                state.LoadOrder.PriorityOrder.Weapon().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IWeaponGetter>(fk),
                w => state.PatchMod.Weapons.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IWeaponGetter, Weapon>("Name", i => i.Name, (i, v) => i.Name = v),
                    new TranslatedField<IWeaponGetter, Weapon>("Description", i => i.Description, (i, v) => i.Description = v)
                },
                target, log);

            TranslationEngine.Process<IIngestibleGetter, Ingestible>(
                state, Name,
                state.LoadOrder.PriorityOrder.Ingestible().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IIngestibleGetter>(fk),
                w => state.PatchMod.Ingestibles.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IIngestibleGetter, Ingestible>("Name", i => i.Name, (i, v) => i.Name = v),
                    new TranslatedField<IIngestibleGetter, Ingestible>("Description", i => i.Description, (i, v) => i.Description = v)
                },
                target, log);

            TranslationEngine.Process<IScrollGetter, Scroll>(
                state, Name,
                state.LoadOrder.PriorityOrder.Scroll().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IScrollGetter>(fk),
                w => state.PatchMod.Scrolls.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IScrollGetter, Scroll>("Name", i => i.Name, (i, v) => i.Name = v),
                    new TranslatedField<IScrollGetter, Scroll>("Description", i => i.Description, (i, v) => i.Description = v)
                },
                target, log);

            TranslationEngine.Process<IIngredientGetter, Ingredient>(
                state, Name,
                state.LoadOrder.PriorityOrder.Ingredient().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IIngredientGetter>(fk),
                w => state.PatchMod.Ingredients.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IIngredientGetter, Ingredient>("Name", i => i.Name, (i, v) => i.Name = v)
                },
                target, log);

            TranslationEngine.Process<IMiscItemGetter, MiscItem>(
                state, Name,
                state.LoadOrder.PriorityOrder.MiscItem().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IMiscItemGetter>(fk),
                w => state.PatchMod.MiscItems.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IMiscItemGetter, MiscItem>("Name", i => i.Name, (i, v) => i.Name = v)
                },
                target, log);

            TranslationEngine.Process<IKeyGetter, Key>(
                state, Name,
                state.LoadOrder.PriorityOrder.Key().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<IKeyGetter>(fk),
                w => state.PatchMod.Keys.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<IKeyGetter, Key>("Name", i => i.Name, (i, v) => i.Name = v)
                },
                target, log);

            TranslationEngine.Process<ISoulGemGetter, SoulGem>(
                state, Name,
                state.LoadOrder.PriorityOrder.SoulGem().WinningOverrides(),
                fk => state.LinkCache.ResolveAll<ISoulGemGetter>(fk),
                w => state.PatchMod.SoulGems.GetOrAddAsOverride(w),
                new[]
                {
                    new TranslatedField<ISoulGemGetter, SoulGem>("Name", i => i.Name, (i, v) => i.Name = v)
                },
                target, log);
        }
    }
}
