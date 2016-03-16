using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Quickness Item Effect")]
    public class QuicknessItemEffect : ItemEffect
    {
        [SerializeField]
        private float duration;

        public float Duration
        {
            get
            {
                return duration;
            }
        }
    }
}
