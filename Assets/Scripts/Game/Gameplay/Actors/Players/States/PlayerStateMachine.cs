using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerStateMachine : StateMachine<PlayerStateMachine, PlayerBaseState, PlayerStateType, PlayerStateFactory>
    {
        private readonly Settings settings;
        private readonly Input input;
        private readonly Camera camera;
        private readonly Grounding grounding;
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;

        private Vector3 lookDirection;
        private Vector3 moveDirection;

        public PlayerStateMachine(
                Settings settings,
                Input input,
                Camera camera,
                Grounding grounding,
                PlayerView playerView,
                PlayerController playerController,
                PlayerStateFactory stateFactory)
            : base(stateFactory)
        {
            this.settings = settings;
            this.input = input;
            this.camera = camera;
            this.grounding = grounding;
            this.playerView = playerView;
            this.playerController = playerController;
            this.playerController.playerStateMachine = this;
        }

        public Vector3 LookDirection
        {
            get
            {
                return lookDirection;
            }
        }

        public Vector3 MoveDirection
        {
            get
            {
                return moveDirection;
            }
            set
            {
                moveDirection = value;
            }
        }

        [PostInject]
        public void InitializeState()
        {
            ChangeState(PlayerStateType.Idle);
            lookDirection = playerView.Forward;
        }

        protected override void EarlyGlobalUpdate() {
            if (input.Mapping.Direction.Vector.magnitude == 0) {
                return;
            }

            Vector3 viewportOrigin = camera.WorldToViewportPoint(playerView.Position);
            viewportOrigin.z = camera.nearClipPlane;

            Vector3 viewportPoint = new Vector3(
                Mathf.Clamp(viewportOrigin.x + (input.Mapping.Direction.X * settings.tolerance), 0f, 1f),
                Mathf.Clamp(viewportOrigin.y + (input.Mapping.Direction.Y * settings.tolerance), 0f, 1f),
                camera.nearClipPlane
            );

            float rayDistance;
            Ray ray = camera.ViewportPointToRay(viewportPoint);
            Plane feetPlanar = new Plane(playerView.Up, playerView.Position);

            if (feetPlanar.Raycast(ray, out rayDistance)) {
                lookDirection = (ray.GetPoint(rayDistance) - playerView.Position).normalized;

                if (settings.Debug.showLookingDirection)
                {
                    Debug.DrawLine(camera.ViewportToWorldPoint(viewportOrigin), camera.ViewportToWorldPoint(viewportPoint), Color.black);

                    Debug.DrawLine(playerView.Position, playerView.Position + lookDirection, Color.black);

                    Debug.DrawLine(camera.ViewportToWorldPoint(viewportPoint), ray.GetPoint(rayDistance), Color.black);

                    Debug.DrawLine(playerView.Position, ray.GetPoint(rayDistance), Color.black);
                }
            }
        }

        protected override void LateGlobalUpdate() {
            ApplyFriction();

            playerView.Position += moveDirection * playerController.DeltaTime;
            playerView.MeshTransform.rotation = Quaternion.LookRotation(lookDirection, playerView.Up);
        }

        private void ApplyFriction()
        {
            if (playerController.MaintainingGround() && grounding.Collidable != null)
            {
                float friction = grounding.Collidable.Friction;
                moveDirection = Vector3.MoveTowards(
                    moveDirection,
                    Vector3.zero,
                    friction * playerController.DeltaTime
                );
            }
        }

        [Serializable]
        public class Settings
        {
            public DebugSettings Debug;
            public float tolerance;

            [Serializable]
            public class DebugSettings
            {
                public bool showLookingDirection;
            }
        }
    }
}
