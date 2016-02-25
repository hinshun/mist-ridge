using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
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
    }
}
