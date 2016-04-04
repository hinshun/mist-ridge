using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class PlayerControlSignal : Signal<PlayerView, bool>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
