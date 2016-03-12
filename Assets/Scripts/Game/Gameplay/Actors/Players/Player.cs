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

        private bool isAlive;

        private float rotationSpeed;
        private float walkSpeed;
        private float walkAcceleration;
        private float walkThreshold;
        private float jumpSpeed;
        private float jumpSpeedLimit;
        private float jumpHeight;
        private float jumpAcceleration;
        private float freefallSpeed;
        private float freefallAcceleration;
        private float freefallDrag;
        private float freefallTilt;
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

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;
                playerView.SetActive(value);
            }
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

        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }
            set
            {
                rotationSpeed = value;
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

        public float WalkThreshold
        {
            get
            {
                return walkThreshold;
            }
            set
            {
                walkThreshold = value;
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

        public float JumpSpeedLimit
        {
            get
            {
                return jumpSpeedLimit;
            }
            set
            {
                jumpSpeedLimit = value;
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

        public float FreefallSpeed
        {
            get
            {
                return freefallSpeed;
            }
            set
            {
                freefallSpeed = value;
            }
        }

        public float FreefallAcceleration
        {
            get
            {
                return freefallAcceleration;
            }
            set
            {
                freefallAcceleration = value;
            }
        }

        public float FreefallDrag
        {
            get
            {
                return freefallDrag;
            }
            set
            {
                freefallDrag = value;
            }
        }

        public float FreefallTilt
        {
            get
            {
                return freefallTilt;
            }
            set
            {
                freefallTilt = value;
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

        public float CurrentRotationSpeed
        {
            get
            {
                return rotationSpeed * playerPhysics.RotationSpeed;
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
        }

        public float CurrentWalkThreshold
        {
            get
            {
                return walkThreshold * playerPhysics.WalkThreshold;
            }
        }

        public float CurrentJumpSpeed
        {
            get
            {
                return jumpSpeed * playerPhysics.JumpSpeed;
            }
        }

        public float CurrentJumpSpeedLimit
        {
            get
            {
                return jumpSpeedLimit * playerPhysics.JumpSpeedLimit;
            }
        }

        public float CurrentJumpHeight
        {
            get
            {
                return jumpHeight * playerPhysics.JumpHeight;
            }
        }

        public float CurrentJumpAcceleration
        {
            get
            {
                return jumpAcceleration * playerPhysics.JumpAcceleration;
            }
        }

        public float CurrentFreefallSpeed
        {
            get
            {
                return freefallSpeed * playerPhysics.FreefallSpeed;
            }
        }

        public float CurrentFreefallAcceleration
        {
            get
            {
                return freefallAcceleration * playerPhysics.FreefallAcceleration;
            }
        }

        public float CurrentFreefallDrag
        {
            get
            {
                return freefallDrag * playerPhysics.FreefallDrag;
            }
        }

        public float CurrentFreefallTilt
        {
            get
            {
                return freefallTilt * playerPhysics.FreefallTilt;
            }
        }

        public float CurrentGravity
        {
            get
            {
                return gravity * playerPhysics.Gravity;
            }
        }

        public void Initialize()
        {
            ResetPlayerProperties();
            ResetPlayerMaterials(input.DeviceNum);
        }

        private void ResetPlayerProperties()
        {
            isAlive = true;
            rotationSpeed = 1f;
            walkSpeed = 1f;
            walkAcceleration = 1f;
            walkThreshold = 1f;
            jumpSpeed = 1f;
            jumpSpeedLimit = 1f;
            jumpHeight = 1f;
            jumpAcceleration = 1f;
            freefallSpeed = 1f;
            freefallAcceleration = 1f;
            freefallDrag = 1f;
            freefallTilt = 1f;
            gravity = 1f;
        }

        private void ResetPlayerMaterials(int deviceNum)
        {
        }

        [Serializable]
        public class Settings
        {
            public Material[] playerMaterials;
        }
    }
}
