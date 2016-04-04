using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class DialogueStateSignal : Signal<DialogueStateType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
