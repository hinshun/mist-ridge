using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Players/Player Physics")]
    public class PlayerPhysics : ScriptableObject
    {
        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float walkAcceleration;

        [SerializeField]
        private float jumpSpeed;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private float jumpAcceleration;

        [SerializeField]
        private float gravity;

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

        public float Gravity
        {
            get
            {
                return gravity;
            }
        }
    }
}
