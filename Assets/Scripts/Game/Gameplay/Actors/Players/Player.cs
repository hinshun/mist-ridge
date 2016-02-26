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
            PlayerPhysics playerPhysics)
        {
            this.settings = settings;
            this.input = input;
            this.playerView = playerView;
            this.playerPhysics = playerPhysics;
        }

        public float WalkSpeed
        {
            get
            {
                return walkSpeed * playerPhysics.WalkSpeed;
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
                return walkAcceleration * playerPhysics.WalkAcceleration;
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
                return jumpSpeed * playerPhysics.JumpSpeed;
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
                return jumpHeight * playerPhysics.JumpHeight;
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
                return jumpAcceleration * playerPhysics.JumpAcceleration;
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
