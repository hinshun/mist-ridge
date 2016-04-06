using System;

namespace MistRidge
{
    [Serializable]
    public struct Dialogue
    {
        public String text;
        public DialogueEvent dialogueEvent;
        public float zoomOverride;
    }
}
