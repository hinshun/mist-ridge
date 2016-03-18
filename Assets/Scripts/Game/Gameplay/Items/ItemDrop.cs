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

        [SerializeField]
        private int rarity;

        [SerializeField]
        private ItemCollectability itemCollectability = new ItemCollectability() {
            firstCollectable = true,
            secondCollectable = true,
            thirdCollectable = true,
            fourthCollectable = true,
        };

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

        public int Rarity
        {
            get
            {
                return rarity;
            }
        }

        public ItemCollectability ItemCollectability
        {
            get
            {
                return itemCollectability;
            }
        }
    }
}
