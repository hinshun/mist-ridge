using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Aether Item Effect")]
    public class AetherItemEffect : ItemEffect
    {
        [SerializeField]
        private int aetherCount;

        public int AetherCount
        {
            get
            {
                return aetherCount;
            }
        }
    }
}
