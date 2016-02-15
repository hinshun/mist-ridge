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
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;

        private Vector3 lookDirection;
        private Vector3 moveDirection;

        public PlayerStateMachine(
                Settings settings,
                Input input,
                Camera camera,
                PlayerView playerView,
                PlayerController playerController,
                PlayerStateFactory stateFactory)
            : base(stateFactory)
        {
            this.settings = settings;
            this.input = input;
            this.camera = camera;
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

        public void ChangeState(PlayerStateType stateType)
        {
            ChangeState(stateType, input, this, playerView, playerController);
        }

        protected override void EarlyGlobalUpdate() {
            if (input.Current.moveDirection.Vector.magnitude == 0) {
                return;
            }

            Vector3 viewportOrigin = camera.WorldToViewportPoint(playerView.Position);
            viewportOrigin.z = camera.nearClipPlane;

            Vector3 viewportPoint = new Vector3(
                Mathf.Clamp(viewportOrigin.x + input.Current.moveDirection.X, 0f, 1f),
                Mathf.Clamp(viewportOrigin.y + input.Current.moveDirection.Y, 0f, 1f),
                camera.nearClipPlane
            );

            float rayDistance;
            Ray ray = camera.ViewportPointToRay(viewportPoint);
            Plane feetPlanar = new Plane(playerView.Up, playerView.Position);

            if (feetPlanar.Raycast(ray, out rayDistance)) {
                lookDirection = (ray.GetPoint(rayDistance) - playerView.Position).normalized;

                if (settings.Debug.showLookingDirection)
                {
                    Debug.DrawLine(playerView.Position, playerView.Position + lookDirection, settings.Debug.lookingDirectionColor);
                }
            }
        }

        protected override void LateGlobalUpdate() {
			if ((moveDirection * playerController.DeltaTime).y < -0.4) {
				Debug.Log ("goodness");
			}
            playerView.Position += moveDirection * playerController.DeltaTime;
            playerView.MeshTransform.rotation = Quaternion.LookRotation(lookDirection, playerView.Up);
        }

        [Serializable]
        public class Settings
        {
            public DebugSettings Debug;

            [Serializable]
            public class DebugSettings
            {
                public bool showLookingDirection;
                public Color lookingDirectionColor;
            }
        }
    }
}
