using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class DialogueSignal : Signal<DialogueType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
