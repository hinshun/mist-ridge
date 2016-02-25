using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class StarItem : ConsumableItem<StarItemEffect>
    {
        public void Consume()
        {
            Debug.Log("Star item used");
        }
    }
}
