using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class TutorialSignal : Signal<TutorialType, PlayerView>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
