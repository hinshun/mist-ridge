using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Bubble Trap Item Effect")]
    public class BubbleTrapItemEffect : ItemEffect
    {
        [SerializeField]
        private BubbleTrapView bubbleTrapView;

        [SerializeField]
        private float distance;

        public BubbleTrapView BubbleTrapView
        {
            get
            {
                return bubbleTrapView;
            }
        }

        public float Distance
        {
            get
            {
                return distance;
            }
        }
    }
}
