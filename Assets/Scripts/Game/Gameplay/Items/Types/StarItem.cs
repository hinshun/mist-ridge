using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class StarItem : ConsumableItem<StarItemEffect>
    {
        public StarItem(
                Player player,
                StarItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }
    }
}
