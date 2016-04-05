using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Player : IInitializable
    {
        private readonly Settings settings;
        private readonly Input input;
        private readonly Grounding grounding;
        private readonly AetherManager aetherManager;
        private readonly PlayerView playerView;
        private readonly PlayerStateMachine playerStateMachine;
        private readonly PlayerPhysics playerPhysics;
        private readonly ItemEffectSignal itemEffectSignal;

        private bool isAlive;
        private int currentRank;

        private float rotationSpeed;
        private float walkSpeed;
        private float walkAcceleration;
        private float walkThreshold;
        private float jumpSpeed;
        private float jumpSpeedLimit;
        private float jumpHeight;
        private float jumpAcceleration;
        private float jumpHaltSpeed;
        private float haltSpeed;
        private float haltThreshold;
        private float freefallSpeed;
        private float freefallAcceleration;
        private float freefallDrag;
        private float freefallTilt;
        private float gravity;

        public Player(
            Settings settings,
            Input input,
            Grounding grounding,
            AetherManager aetherManager,
            PlayerView playerView,
            PlayerStateMachine playerStateMachine,
            PlayerPhysics playerPhysics,
            ItemEffectSignal itemEffectSignal)
        {
            this.settings = settings;
            this.input = input;
            this.grounding =  grounding;
            this.aetherManager = aetherManager;
            this.playerView = playerView;
            this.playerStateMachine = playerStateMachine;
            this.playerPhysics = playerPhysics;
            this.itemEffectSignal = itemEffectSignal;
        }

        public PlayerView PlayerView
        {
            get
            {
                return playerView;
            }
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
                playerView.enabled = value;
                playerView.PlayerCircle.enabled = value;
                playerView.MeshTransform.gameObject.SetActive(value);
            }
        }

        public int CurrentRank
        {
            get
            {
                return currentRank;
            }
            set
            {
                currentRank = value;
            }
        }

        public Vector3 PrimaryNormal
        {
            get
            {
                return grounding.PrimaryNormal;
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

        public Vector3 Forward
        {
            get
            {
                return playerView.Forward;
            }
        }

        public Vector3 HandPosition
        {
            get
            {
                return playerView.HandTransform.position;
            }
        }

        public Vector3 Position
        {
            get
            {
                return playerView.Position;
            }
            set
            {
                playerView.Position = value;
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

        public float JumpHaltSpeed
        {
            get
            {
                return jumpHaltSpeed;
            }
            set
            {
                jumpHaltSpeed = value;
            }
        }

        public float HaltSpeed
        {
            get
            {
                return haltSpeed;
            }
            set
            {
                haltSpeed = value;
            }
        }

        public float HaltThreshold
        {
            get
            {
                return haltThreshold;
            }
            set
            {
                haltThreshold = value;
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

        public float CurrentJumpHaltSpeed
        {
            get
            {
                return jumpHaltSpeed * playerPhysics.JumpHaltSpeed;
            }
        }

        public float CurrentHaltSpeed
        {
            get
            {
                return haltSpeed * playerPhysics.HaltSpeed;
            }
        }

        public float CurrentHaltThreshold
        {
            get
            {
                return haltThreshold * playerPhysics.HaltThreshold;
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
            itemEffectSignal.Event += OnItemEffect;
        }

        public void AfterImage()
        {
            playerView.PlayerAfterImage();
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
            jumpHaltSpeed = 1f;
            haltSpeed = 1f;
            haltThreshold = 1f;
            freefallSpeed = 1f;
            freefallAcceleration = 1f;
            freefallDrag = 1f;
            freefallTilt = 1f;
            gravity = 1f;
        }

        private void ResetPlayerMaterials(int deviceNum)
        {
        }

        private void OnItemEffect(ItemType itemType, ItemStage itemStage)
        {
            switch (itemType)
            {
                case ItemType.BubbleTrap:
                    OnBubbleTrap(itemStage);
                    break;
            }
        }

        private void OnBubbleTrap(ItemStage itemStage)
        {
            switch (itemStage)
            {
                case ItemStage.Start:
                    gravity = 0f;
                    playerStateMachine.ChangeState(PlayerStateType.Floating);
                    break;

                case ItemStage.End:
                    gravity = 1f;
                    playerStateMachine.ChangeState(PlayerStateType.Fall);
                    break;
            }
        }

        [Serializable]
        public class Settings
        {
            public Material[] playerMaterials;
        }
    }
}
