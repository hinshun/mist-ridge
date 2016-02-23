using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private ItemType itemType;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }
        }
    }
}
