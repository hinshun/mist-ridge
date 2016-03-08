using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Players/Player Physics")]
    public class PlayerPhysics : ScriptableObject
    {
        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float walkAcceleration;

        [SerializeField]
        private float walkThreshold;

        [SerializeField]
        private float jumpSpeed;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private float jumpAcceleration;

        [SerializeField]
        private float freefallSpeed;

        [SerializeField]
        private float freefallAcceleration;

        [SerializeField]
        private float freefallDrag;

        [SerializeField]
        private float freefallTilt;

        [SerializeField]
        private float gravity;

        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }
        }

        public float WalkSpeed
        {
            get
            {
                return walkSpeed;
            }
        }

        public float WalkAcceleration
        {
            get
            {
                return walkAcceleration;
            }
        }

        public float WalkThreshold
        {
            get
            {
                return walkThreshold;
            }
        }

        public float JumpSpeed
        {
            get
            {
                return jumpSpeed;
            }
        }

        public float JumpHeight
        {
            get
            {
                return jumpHeight;
            }
        }

        public float JumpAcceleration
        {
            get
            {
                return jumpAcceleration;
            }
        }

        public float FreefallSpeed
        {
            get
            {
                return freefallSpeed;
            }
        }

        public float FreefallAcceleration
        {
            get
            {
                return freefallAcceleration;
            }
        }

        public float FreefallDrag
        {
            get
            {
                return freefallDrag;
            }
        }

        public float FreefallTilt
        {
            get
            {
                return freefallTilt;
            }
        }

        public float Gravity
        {
            get
            {
                return gravity;
            }
        }
    }
}
