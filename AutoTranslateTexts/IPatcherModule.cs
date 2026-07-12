using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace AutoTranslateTexts
{
    /// <summary>
    /// A self-contained translation module. Each record type (books, items, names, ...)
    /// is handled by its own module so they can be toggled and logged independently.
    /// </summary>
    public interface IPatcherModule
    {
        /// <summary>Human readable module name, also used for the log file name.</summary>
        string Name { get; }

        /// <summary>Returns the per-module settings block (enabled flag + log flag).</summary>
        ModuleSetting GetSettings(Settings settings);

        /// <summary>Runs the module against the load order.</summary>
        void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, TargetLanguage target, bool log);
    }
}
