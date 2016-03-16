using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class BubbleTrapItem : ConsumableItem<BubbleTrapItemEffect>
    {
        public BubbleTrapItem(
                Player player,
                BubbleTrapItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }
    }
}
