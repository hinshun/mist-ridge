using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class DeathSignal : Signal<PlayerView>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
