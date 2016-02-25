using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpeedItem : ConsumableItem<SpeedItemEffect>
    {
        public SpeedItem(SpeedItemEffect itemEffect)
            : base(itemEffect)
        {
        }

        public override void Consume()
        {
            Debug.Log("Speed item used");
        }
    }
}
