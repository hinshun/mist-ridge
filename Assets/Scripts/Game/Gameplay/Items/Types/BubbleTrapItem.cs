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
            BubbleTrapView bubbleTrapView = poolManager.ReusePoolInstance(
                itemEffect.BubbleTrapView,
                player.HandPosition,
                Quaternion.identity
            ) as BubbleTrapView;

            Vector3 landingPosition = Math3d.ProjectVectorOnPlane(
                player.PrimaryNormal,
                player.Position + player.Forward * itemEffect.Distance
            );
            bubbleTrapView.Land(landingPosition);
        }
    }
}
