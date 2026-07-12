using Mutagen.Bethesda.Synthesis.Settings;

namespace AutoTranslateTexts
{
    /// <summary>Per-module toggle plus its own log switch, so modules are configured separately.</summary>
    public class ModuleSetting
    {
        [SynthesisTooltip("Enable this module")]
        public bool Enabled = true;

        [SynthesisTooltip("Write a .log file listing every change this module made")]
        public bool Log = false;
    }

    public class Settings
    {
        [SynthesisTooltip(
            "Target language applied across the load order. Detection is script-based " +
            "(Cyrillic, Chinese, Japanese, Korean, Arabic, Greek). Latin-script languages " +
            "(English, French, German, Italian, Spanish, Polish, ...) all count as 'Latin' and " +
            "cannot be told apart from one another.")]
        public TargetLanguage TargetLanguage = TargetLanguage.Russian;

        [SynthesisTooltip("Books: title, description and body text")]
        public ModuleSetting Books = new() { Enabled = true };

        [SynthesisTooltip("Locations: place names")]
        public ModuleSetting Locations = new() { Enabled = true };

        [SynthesisTooltip("Names: NPC names and short names")]
        public ModuleSetting Names = new() { Enabled = true };

        [SynthesisTooltip("Items: armor, weapons, potions, scrolls, ingredients, misc, keys, soul gems")]
        public ModuleSetting Items = new() { Enabled = true };

        [SynthesisTooltip("Dialogue: topic prompts and NPC response lines")]
        public ModuleSetting Dialogue = new() { Enabled = true };
    }
}
