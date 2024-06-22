// using System;
// using System.Collections.Generic;
// using System.Text;
// using Mutagen.Bethesda.Plugins;
// using Mutagen.Bethesda.Plugins.Records;
// using Mutagen.Bethesda.Skyrim;
// using Mutagen.Bethesda.Synthesis;
//
// namespace AutoTranslateTexts
// {
//     public class DialoguePatcher
//     {
//         public static void Run(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, bool log)
//         {
//             var dialogueTranslations = new Dictionary<FormKey, DialogueTranslation>();
//
//             foreach (var dialogue in state.LoadOrder.PriorityOrder.DialogBranch().WinningOverrides())
//             {
//                 ProcessDialogue(dialogue, dialogueTranslations);
//
//                 if (log)
//                 {
//                     PrintTranslations(dialogueTranslations);
//                 }
//             }
//         }
//
//         private static void ProcessDialogue(Dialog dialogue, Dictionary<FormKey, DialogueTranslation> dialogueTranslations)
//         {
//             var translation = new DialogueTranslation();
//
//             if (dialogue.Name != null)
//             {
//                 translation.Name = EncodingHelper.ConvertToUtf8(dialogue.Name.String);
//             }
//
//             dialogueTranslations.TryAdd(dialogue.FormKey, translation);
//         }
//     }
//
//     public class DialogueTranslation
//     {
//         public string Text { get; set; } = string.Empty;
//     }
// }