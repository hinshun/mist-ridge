using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class ItemEffectSignal : Signal<ItemType, ItemStage>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
