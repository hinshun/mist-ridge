using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class BubbleTrapItem : ConsumableItem<BubbleTrapItemEffect>
    {
        private readonly Settings settings;
        private readonly PoolManager poolManager;

        public BubbleTrapItem(
                Settings settings,
                PoolManager poolManager,
                Player player,
                BubbleTrapItemEffect itemEffect)
            : base(player, itemEffect)
        {
            this.settings = settings;
            this.poolManager = poolManager;
        }

        public override void OnUse()
        {
            BubbleTrapView bubbleTrapView = poolManager.ReusePoolInstance(
                itemEffect.BubbleTrapView,
                player.HandPosition,
                Quaternion.identity
            ) as BubbleTrapView;

            Vector3 landingOrigin = player.Position + (player.Forward * itemEffect.Distance) + Vector3.up;

            RaycastHit hitInfo;

            if (Physics.SphereCast(
                landingOrigin,
                settings.epsilon,
                Vector3.down,
                out hitInfo,
                maxDistance: Mathf.Infinity,
                layerMask: settings.walkableLayerMask
            ))
            {
                bubbleTrapView.Land(hitInfo.point);
            }
            else
            {
                bubbleTrapView.Land(landingOrigin - Vector3.up);
            }
        }

        [Serializable]
        public class Settings
        {
            public float epsilon;
            public LayerMask walkableLayerMask;
        }
    }
}
