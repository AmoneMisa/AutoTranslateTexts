using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public sealed class LocationModule : IPatcherModule
    {
        public string Name => "LocationPatcher";

        public ModuleSetting GetSettings(Settings settings) => settings.Locations;

        public void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log)
        {
            TranslationEngine.Process<ILocationGetter, Location>(
                state,
                Name,
                state.LoadOrder.PriorityOrder.Location().WinningOverrides(),
                formKey => state.LinkCache.ResolveAll<ILocationGetter>(formKey),
                winner => state.PatchMod.Locations.GetOrAddAsOverride(winner),
                new[]
                {
                    new TranslatedField<ILocationGetter, Location>("Name", l => l.Name, (l, v) => l.Name = v)
                },
                target,
                log);
        }
    }
}
