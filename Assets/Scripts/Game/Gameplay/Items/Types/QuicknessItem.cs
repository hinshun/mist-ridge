using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class QuicknessItem : ConsumableItem<QuicknessItemEffect>
    {
        public QuicknessItem(
                Player player,
                QuicknessItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }
    }
}
