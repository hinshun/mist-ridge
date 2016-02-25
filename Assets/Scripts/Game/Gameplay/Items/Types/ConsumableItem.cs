using UnityEngine;
using Zenject;

namespace MistRidge
{
    public abstract class ConsumableItem<TItemEffect> : IItem
        where TItemEffect : ItemEffect
    {
        protected readonly TItemEffect itemEffect;
        protected int uses;

        public ConsumableItem(TItemEffect itemEffect)
        {
            this.itemEffect = itemEffect;
        }

        public abstract void Consume();

        public void Initialize()
        {
            uses = itemEffect.MaxUses;
        }

        public virtual void Tick()
        {
            // Do Nothing
        }

        public virtual void Dispose()
        {
            // Do Nothing
        }

        public void Use()
        {
            uses--;
            Consume();
        }

        public bool IsUsable()
        {
            return uses > 0;
        }
    }
}
