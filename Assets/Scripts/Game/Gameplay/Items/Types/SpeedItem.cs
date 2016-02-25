using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpeedItem : IItem
    {
        private readonly SpeedItemEffect itemEffect;

        private int uses;

        public SpeedItem(SpeedItemEffect itemEffect)
        {
            this.itemEffect = itemEffect;
        }

        public void Initialize()
        {
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
            Debug.Log("Speed item used");
        }

        public bool IsUsable()
        {
            return uses > 0;
        }
    }
}
