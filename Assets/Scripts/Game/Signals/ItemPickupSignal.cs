using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class ItemPickupSignal : Signal<ItemType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
