using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class AetherItem : ConsumableItem<AetherItemEffect>
    {
        public AetherItem(
                Player player,
                AetherItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }

        public override void OnUse()
        {
            player.AddAether(itemEffect.AetherCount);
        }
    }
}
