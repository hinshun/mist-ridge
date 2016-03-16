using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Bubble Trap Item Effect")]
    public class BubbleTrapItemEffect : ItemEffect
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float height;

        public float Duration
        {
            get
            {
                return duration;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
        }
    }
}
