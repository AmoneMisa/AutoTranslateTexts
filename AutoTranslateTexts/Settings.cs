using System.IO;
using Newtonsoft.Json;
using Mutagen.Bethesda.Synthesis.Settings;

namespace AutoTranslateTexts
{
    public class Settings
    {
        [SynthesisTooltip("Show Book Log | 1 - On | 0 - Off")]
        public bool BookLog = false;
        [SynthesisTooltip("Show Locations Log | 1 - On | 0 - Off")]
        public bool LocationsLog = false;
        [SynthesisTooltip("Show Items Log | 1 - On | 0 - Off")]
        public bool ItemsLog = false;
        [SynthesisTooltip("Show Names Log | 1 - On | 0 - Off")]
        public bool NamesLog = false;
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