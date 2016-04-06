using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class DialogueManager : IInitializable, IDisposable, ITickable
    {
        private readonly Settings settings;
        private readonly DialogueSignal dialogueSignal;
        private readonly DialogueStateSignal dialogueStateSignal;
        private readonly DialogueManagerView dialogueManagerView;
        private readonly DisplayManager displayManager;
        private readonly InputManager inputManager;
        private readonly TutorialSignal.Trigger tutorialTrigger;

        private int dialogueIndex;
        private Dialogue dialogue;
        private DialogueType dialogueType;
        private DialogueStateType dialogueStateType;

        private Hashtable textHashtable;

        public DialogueManager(
                Settings settings,
                DialogueSignal dialogueSignal,
                DialogueStateSignal dialogueStateSignal,
                DialogueManagerView dialogueManagerView,
                DisplayManager displayManager,
                InputManager inputManager,
                TutorialSignal.Trigger tutorialTrigger)
        {
            this.settings = settings;
            this.dialogueSignal = dialogueSignal;
            this.dialogueStateSignal = dialogueStateSignal;
            this.dialogueManagerView = dialogueManagerView;
            this.displayManager = displayManager;
            this.inputManager = inputManager;
            this.tutorialTrigger = tutorialTrigger;
        }

        public void Initialize()
        {
            ResetVariables();
            dialogueSignal.Event += OnDialogue;
            dialogueStateSignal.Event += OnDialogueState;
            dialogueManagerView.DialogueManager = this;

            textHashtable = new Hashtable();
            textHashtable.Add("delay", settings.textDelay);
            textHashtable.Add("onupdate", "OnUpdateText");
            textHashtable.Add("oncomplete", "OnFinishText");
        }

        public void Dispose()
        {
            dialogueSignal.Event -= OnDialogue;
            dialogueStateSignal.Event -= OnDialogueState;
        }

        public void ResetVariables()
        {
            dialogueIndex = 0;
            dialogueType = DialogueType.None;
        }

        public void Tick()
        {
            if (dialogueStateType != DialogueStateType.TextFinish)
            {
                return;
            }

            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.Submit.WasPressed)
                {
                    dialogueStateType = DialogueStateType.TextStart;
                    dialogueIndex++;
                    displayManager.UpdateDialogueNext(false);

                    List<Dialogue> dialogues = GetDialogues(dialogueType);
                    DisplayDialogues(dialogues);

                    return;
                }
            }
        }

        public void OnUpdateText(float index)
        {
            String text = dialogue.text.Substring(0, Mathf.CeilToInt(index));
            displayManager.UpdateDialogueText(text);
        }

        public void OnFinishText()
        {
            displayManager.UpdateDialogueNext(true);
            dialogueStateType = DialogueStateType.TextFinish;
        }

        private void OnDialogue(DialogueType dialogueType)
        {
            this.dialogueType = dialogueType;
        }

        private void OnDialogueState(DialogueStateType dialogueStateType)
        {
            switch(dialogueStateType)
            {
                case DialogueStateType.Start:
                    OnDialogueStart();
                    break;
            }
        }

        private void OnDialogueStart()
        {
            dialogueIndex = 0;
            List<Dialogue> dialogues = GetDialogues(dialogueType);

            dialogueStateType = DialogueStateType.TextStart;
            DisplayDialogues(dialogues);
        }

        private void DisplayDialogues(List<Dialogue> dialogues)
        {
            if (dialogueIndex > dialogues.Count - 1)
            {
                OnDialogueEnd();
                return;
            }

            dialogue = dialogues[dialogueIndex];

            textHashtable["from"] = 0;
            textHashtable["to"] = dialogue.text.Length;
            textHashtable["time"] = dialogue.text.Length * settings.textSpeed;

            iTween.ValueTo(dialogueManagerView.gameObject, textHashtable);
            displayManager.UpdateDialogueText("");
            displayManager.UpdateDialogue(true);
        }

        private void OnDialogueEnd()
        {
            dialogueStateType = DialogueStateType.End;
            displayManager.UpdateDialogue(false);

            if (dialogueType == DialogueType.TurnipTutorial)
            {
                tutorialTrigger.Fire(TutorialType.End);
            }
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
            public float textSpeed;
            public float textDelay;
        }
    }
}
