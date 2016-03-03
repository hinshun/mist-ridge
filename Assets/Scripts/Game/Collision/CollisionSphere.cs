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
                CollisionSphereRequest request)
        {
            this.playerView = playerView;
            radius = request.radius;
            offset = request.offset;
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

        public class Factory : Factory<PlayerView, CollisionSphereRequest, CollisionSphere>
        {
        }
    }
}
