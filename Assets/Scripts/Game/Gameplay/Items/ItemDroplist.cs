using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Droplist")]
    public class ItemDroplist : ScriptableObject
    {
        [SerializeField]
        private List<ItemDrop> itemDrops;

        public List<ItemDrop> ItemDrops
        {
            get
            {
                return itemDrops;
            }
        }
    }
}
