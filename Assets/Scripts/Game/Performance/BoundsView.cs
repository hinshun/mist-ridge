using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class BoundsView : MonoView
    {
        private CollisionSignal.Trigger collisionTrigger;

        [PostInject]
        public void Init(CollisionSignal.Trigger collisionTrigger)
        {
            this.collisionTrigger = collisionTrigger;
        }

        private void OnTriggerEnter(Collider other)
        {
            collisionTrigger.Fire(CollisionType.TriggerEnter, other);
        }

        private void OnTriggerExit(Collider other)
        {
            collisionTrigger.Fire(CollisionType.TriggerExit, other);
        }
    }
}
