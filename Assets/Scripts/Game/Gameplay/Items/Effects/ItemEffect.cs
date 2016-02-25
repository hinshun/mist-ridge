using UnityEngine;

namespace MistRidge
{
    [Serializable]
    public abstract class ItemEffect
    {
        public abstract void GetFactory(Player player);

        public abstract void Use();

        public abstract void Tick();
    }
}
