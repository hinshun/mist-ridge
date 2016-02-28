using UnityEngine;
using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class CollisionSignal : Signal<CollisionType, Collider>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
