using UnityEngine;
using Zenject;

namespace MistRidge
{
    public abstract class ConsumableItem<TItemEffect> : IItem
        where TItemEffect : ItemEffect
    {
        protected readonly Player player;
        protected readonly TItemEffect itemEffect;

        protected bool isActive;
        protected bool isDisposable;
        protected int uses;

        public ConsumableItem(
                Player player,
                TItemEffect itemEffect)
        {
            this.player = player;
            this.itemEffect = itemEffect;
        }

        public virtual void Initialize()
        {
            isActive = false;
            isDisposable = false;
            uses = itemEffect.MaxUses;
        }

        public virtual void Tick()
        {
            if (!isDisposable && !IsUsable())
            {
                isDisposable = true;
            }
        }

        public virtual void Dispose()
        {
            // Do Nothing
        }

        public void Use()
        {
            uses--;
            isActive = true;
            OnUse();
        }

        public bool IsUsable()
        {
            return uses > 0;
        }

        public virtual void OnUse()
        {
            // Do Nothing
        }

        public bool IsActive()
        {
            return isActive;
        }

        public bool IsDisposable()
        {
            return isDisposable;
        }
    }
}
