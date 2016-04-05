using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class AetherItem : ConsumableItem<AetherItemEffect>
    {
        private readonly AetherGainSignal.Trigger aetherGainTrigger;

        public AetherItem(
                AetherGainSignal.Trigger aetherGainTrigger,
                Player player,
                AetherItemEffect itemEffect)
            : base(player, itemEffect)
        {
            this.aetherGainTrigger = aetherGainTrigger;
        }

        public override void OnUse()
        {
            for (int i = 0; i < itemEffect.AetherCount; ++i)
            {
                aetherGainTrigger.Fire(player.PlayerView);
            }
        }
    }
}
