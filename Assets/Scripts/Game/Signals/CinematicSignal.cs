using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class CinematicSignal : Signal<CinematicTransitionType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
