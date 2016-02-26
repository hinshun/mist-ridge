using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Speed Item Effect")]
    public class SpeedItemEffect : ItemEffect
    {
        [SerializeField]
        private float speedBoost;

        [SerializeField]
        private float duration;

        public float SpeedBoost
        {
            get
            {
                return speedBoost;
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }
        }
    }
}
