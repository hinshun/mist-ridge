using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class ItemGainSignal : Signal<PlayerView, ItemType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
