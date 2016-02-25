using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpeedItem : ConsumableItem<SpeedItemEffect>
    {
        public void Consume()
        {
            Debug.Log("Speed item used");
        }
    }
}
