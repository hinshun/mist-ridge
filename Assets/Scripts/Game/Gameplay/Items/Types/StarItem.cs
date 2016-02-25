using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class StarItem : ConsumableItem<StarItemEffect>
    {
        public StarItem(StarItemEffect itemEffect)
            : base(itemEffect)
        {
        }

        public override void Consume()
        {
            Debug.Log("Star item used");
        }
    }
}
