using UnityEngine;
using System;

namespace MistRidge
{
    public class DialogueManagerView : MonoView
    {
        private DialogueManager dialogueManager;

        public DialogueManager DialogueManager
        {
            get
            {
                return dialogueManager;
            }
            set
            {
                dialogueManager = value;
            }
        }

        public void OnUpdateText(float index)
        {
            dialogueManager.OnUpdateText(index);
        }

        public void OnFinishText()
        {
            dialogueManager.OnFinishText();
        }
    }
}
