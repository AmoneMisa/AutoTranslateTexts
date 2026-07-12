using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public sealed class NameModule : IPatcherModule
    {
        public string Name => "NamePatcher";

        public ModuleSetting GetSettings(Settings settings) => settings.Names;

        public void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log)
        {
            TranslationEngine.Process<INpcGetter, Npc>(
                state,
                Name,
                state.LoadOrder.PriorityOrder.Npc().WinningOverrides(),
                formKey => state.LinkCache.ResolveAll<INpcGetter>(formKey),
                winner => state.PatchMod.Npcs.GetOrAddAsOverride(winner),
                new[]
                {
                    new TranslatedField<INpcGetter, Npc>("Name", n => n.Name, (n, v) => n.Name = v),
                    new TranslatedField<INpcGetter, Npc>("ShortName", n => n.ShortName, (n, v) => n.ShortName = v)
                },
                target,
                log);
        }
    }
}
