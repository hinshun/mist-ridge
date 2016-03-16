using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class BubbleTrapItem : ConsumableItem<BubbleTrapItemEffect>
    {
        private readonly PoolManager poolManager;

        public BubbleTrapItem(
                PoolManager poolManager,
                Player player,
                BubbleTrapItemEffect itemEffect)
            : base(player, itemEffect)
        {
            this.poolManager = poolManager;
        }

        public override void OnUse()
        {
            poolManager.ReusePoolInstance(
                itemEffect.BubbleTrapView,
                player.Position + (player.Forward * itemEffect.Distance),
                Quaternion.identity
            );
        }
    }
}
