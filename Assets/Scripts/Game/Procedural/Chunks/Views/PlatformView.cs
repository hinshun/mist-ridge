using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlatformView : MonoView
    {
        public class Factory : GameObjectFactory<PlatformView>
        {
        }
    }
}
