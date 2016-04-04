using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class TutorialSignal : Signal<TutorialType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
