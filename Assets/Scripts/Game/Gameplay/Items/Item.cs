using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject, ITickable
    {
        [SerializeField]
        private ItemType itemType;

        [SerializeField]
        private ItemEffect itemEffect;

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }
        }

        public ItemEffect GetEffectInstance(Player player)
        {
            return itemEffect.GetFactory().Create(Player player);
        }

        public void Use(Player player)
        {
            itemEffect.Use(player);
        }

        public void Tick()
        {
            itemEffect.Tick();
        }
    }
}
