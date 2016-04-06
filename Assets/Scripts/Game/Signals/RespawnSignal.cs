using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class RespawnSignal : Signal<PlayerFacade>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
