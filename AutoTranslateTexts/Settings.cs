using System.IO;
using Newtonsoft.Json;
using Mutagen.Bethesda.Synthesis.Settings;

namespace AutoTranslateTexts
{
    public class Settings
    {
        [SynthesisTooltip("Show Log | 1 - On | 0 - Off")]
        public bool Log = false;
        [SynthesisTooltip("Books | 1 - On | 0 - Off")]
        public bool Books = true;
        [SynthesisTooltip("Locations | 1 - On | 0 - Off")]
        public bool Locations = true;
        [SynthesisTooltip("Items | 1 - On | 0 - Off")]
        public bool Items = true;
        [SynthesisTooltip("Names | 1 - On | 0 - Off")]
        public bool Names = true;
        // [SynthesisTooltip("Dialogues | 1 - On | 0 - Off")]
        // public bool Dialogues = true;
    }
}