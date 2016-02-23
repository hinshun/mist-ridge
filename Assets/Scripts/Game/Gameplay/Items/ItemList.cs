using UnityEngine;
using System.Collections.Generic;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item List")]
    public class ItemList : ScriptableObject
    {
        [SerializeField]
        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}
