using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Time Slow Item Effect")]
    public class TimeSlowItemEffect : ItemEffect
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float timeSlow;

        public float Duration
        {
            get
            {
                return duration;
            }
        }

        public float TimeSlow
        {
            get
            {
                return timeSlow;
            }
        }
    }
}
