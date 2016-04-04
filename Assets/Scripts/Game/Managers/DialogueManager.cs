using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class DialogueManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly DialogueSignal dialogueSignal;
        private readonly DialogueStateSignal dialogueStateSignal;

        private DialogueType dialogueType;

        public DialogueManager(
                Settings settings,
                DialogueSignal dialogueSignal,
                DialogueStateSignal dialogueStateSignal)
        {
            this.settings = settings;
            this.dialogueSignal = dialogueSignal;
            this.dialogueStateSignal = dialogueStateSignal;
        }

        public void Initialize()
        {
            dialogueType = DialogueType.None;
            dialogueSignal.Event += OnDialogue;
            dialogueStateSignal.Event += OnDialogueState;
        }

        public void Tick()
        {
            // Do Nothing
        }

        private void OnDialogue(DialogueType dialogueType)
        {
            this.dialogueType = dialogueType;
        }

        private void OnDialogueState(DialogueStateType dialogueStateType)
        {
            Debug.Log("started dialogue: " + dialogueType);
        }

        private List<Dialogue> GetDialogues(DialogueType dialogueType)
        {
            switch(dialogueType)
            {
                case DialogueType.TurnipTutorial:
                    return settings.turnipTutorialDialogues;
            }

            return new List<Dialogue>();
        }

        [Serializable]
        public class Settings
        {
            public List<Dialogue> turnipTutorialDialogues;
        }
    }
}
