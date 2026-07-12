using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    public sealed class BookModule : IPatcherModule
    {
        public string Name => "BookPatcher";

        public ModuleSetting GetSettings(Settings settings) => settings.Books;

        public void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log)
        {
            TranslationEngine.Process<IBookGetter, Book>(
                state,
                Name,
                state.LoadOrder.PriorityOrder.Book().WinningOverrides(),
                formKey => state.LinkCache.ResolveAll<IBookGetter>(formKey),
                winner => state.PatchMod.Books.GetOrAddAsOverride(winner),
                new[]
                {
                    new TranslatedField<IBookGetter, Book>("Name", b => b.Name, (b, v) => b.Name = v),
                    new TranslatedField<IBookGetter, Book>("Description", b => b.Description, (b, v) => b.Description = v),
                    new TranslatedField<IBookGetter, Book>("BookText", b => b.BookText, (b, v) => b.BookText = v)
                },
                target,
                log);
        }
    }
}
