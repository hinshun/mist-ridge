using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Drop")]
    public class ItemDrop : ScriptableObject
    {
        [SerializeField]
        private ItemType itemType;

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }
        }
    }
}
