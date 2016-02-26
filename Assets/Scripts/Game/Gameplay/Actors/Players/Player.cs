using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Player : IInitializable
    {
        private readonly Settings settings;
        private readonly Input input;
        private readonly PlayerView playerView;
        private readonly PlayerStateMachine playerStateMachine;
        private readonly PlayerPhysics playerPhysics;

        private float walkSpeed;
        private float walkAcceleration;
        private float jumpSpeed;
        private float jumpHeight;
        private float jumpAcceleration;
        private float gravity;

        public Player(
            Settings settings,
            Input input,
            PlayerView playerView,
            PlayerStateMachine playerStateMachine,
            PlayerPhysics playerPhysics)
        {
            this.settings = settings;
            this.input = input;
            this.playerView = playerView;
            this.playerStateMachine = playerStateMachine;
            this.playerPhysics = playerPhysics;
        }

        public Vector3 LookDirection
        {
            get
            {
                return playerStateMachine.LookDirection;
            }
        }

        public Vector3 MoveDirection
        {
            get
            {
                return playerStateMachine.MoveDirection;
            }
            set
            {
                playerStateMachine.MoveDirection = value;
            }
        }

        public float WalkSpeed
        {
            get
            {
                return walkSpeed;
            }
            set
            {
                walkSpeed = value;
            }
        }

        public float WalkAcceleration
        {
            get
            {
                return walkAcceleration;
            }
            set
            {
                walkAcceleration = value;
            }
        }

        public float JumpSpeed
        {
            get
            {
                return jumpSpeed;
            }
            set
            {
                jumpSpeed = value;
            }
        }

        public float JumpHeight
        {
            get
            {
                return jumpHeight;
            }
            set
            {
                jumpHeight = value;
            }
        }

        public float JumpAcceleration
        {
            get
            {
                return jumpAcceleration;
            }
            set
            {
                jumpAcceleration = value;
            }
        }

        public float Gravity
        {
            get
            {
                return gravity;
            }
            set
            {
                gravity = value;
            }
        }

        public float CurrentWalkSpeed
        {
            get
            {
                return walkSpeed * playerPhysics.WalkSpeed;
            }
        }

        public float CurrentWalkAcceleration
        {
            get
            {
                return walkAcceleration * playerPhysics.WalkAcceleration;
            }
            set
            {
                walkAcceleration = value;
            }
        }

        public float CurrentJumpSpeed
        {
            get
            {
                return jumpSpeed * playerPhysics.JumpSpeed;
            }
            set
            {
                jumpSpeed = value;
            }
        }

        public float CurrentJumpHeight
        {
            get
            {
                return jumpHeight * playerPhysics.JumpHeight;
            }
            set
            {
                jumpHeight = value;
            }
        }

        public float CurrentJumpAcceleration
        {
            get
            {
                return jumpAcceleration * playerPhysics.JumpAcceleration;
            }
            set
            {
                jumpAcceleration = value;
            }
        }

        public float CurrentGravity
        {
            get
            {
                return gravity * playerPhysics.Gravity;
            }
            set
            {
                gravity = value;
            }
        }

        public void Initialize()
        {
            SetupPlayerProperties();
            SetupPlayerMaterials(input.DeviceNum);
        }

        private void SetupPlayerProperties()
        {
            walkSpeed = 1f;
            walkAcceleration = 1f;
            jumpSpeed = 1f;
            jumpHeight = 1f;
            jumpAcceleration = 1f;
            gravity = 1f;
        }

        private void SetupPlayerMaterials(int deviceNum)
        {
            if (playerView.MeshRenderer != null)
            {
                playerView.MeshRenderer.material = settings.playerMaterials[deviceNum];
            }
        }

        [Serializable]
        public class Settings
        {
            public Material[] playerMaterials;
        }
    }
}
