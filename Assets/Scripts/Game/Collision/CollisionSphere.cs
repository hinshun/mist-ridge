using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class CollisionSphere
    {
        private readonly PlayerView playerView;
        private float radius;
        private float offset;

        public CollisionSphere(
                PlayerView playerView,
                CollisionSphereBlueprint blueprint)
        {
            this.playerView = playerView;
            radius = blueprint.radius;
            offset = blueprint.offset;
        }

        public Vector3 Position
        {
            get
            {
                return playerView.Position + offset * playerView.Up;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public class Factory : Factory<PlayerView, CollisionSphereBlueprint, CollisionSphere>
        {
        }
    }
}
