using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class StarItem : IItem
    {
        private readonly StarItemEffect starItemEffect;

        private int uses;

        public StarItem(StarItemEffect starItemEffect)
        {
            this.starItemEffect = starItemEffect;
        }

        public void Initialize()
        {
            uses = starItemEffect.MaxUses;
        }

        public void Dispose()
        {
        }

        public void Tick()
        {
        }

        public void Use()
        {
            uses--;
            Debug.Log("Star item used");
        }

        public bool IsUsable()
        {
            return uses > 0;
        }
    }
}
