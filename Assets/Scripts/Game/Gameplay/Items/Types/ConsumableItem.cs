using UnityEngine;
using Zenject;

namespace MistRidge
{
    public abstract class ConsumableItem<TItemEffect> : IItem
        where TItemEffect : ItemEffect
    {
        protected readonly Player player;
        protected readonly TItemEffect itemEffect;

        protected int uses;
        protected bool active;

        public ConsumableItem(
                Player player,
                TItemEffect itemEffect)
        {
            this.player = player;
            this.itemEffect = itemEffect;
        }

        public virtual void Initialize()
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
            active = true;
        }

        public bool IsUsable()
        {
            return uses > 0;
        }
    }
}
