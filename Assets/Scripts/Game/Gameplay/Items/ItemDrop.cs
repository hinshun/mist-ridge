using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Drop")]
    public class ItemDrop : ScriptableObject
    {
        [SerializeField]
        private ItemType itemType;

        [SerializeField]
        private Sprite itemSprite;

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }
        }

        public Sprite ItemSprite
        {
            get
            {
                return itemSprite;
            }
        }
    }
}
