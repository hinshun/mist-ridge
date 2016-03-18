using UnityEngine;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Items/Item Effects/Quickness Item Effect")]
    public class QuicknessItemEffect : ItemEffect
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float afterImageDelay;

        [SerializeField]
        private float walkSpeed;

        [SerializeField]
        private float jumpSpeed;

        public float Duration
        {
            get
            {
                return duration;
            }
        }

        public float AfterImageDelay
        {
            get
            {
                return afterImageDelay;
            }
        }

        public float WalkSpeed
        {
            get
            {
                return walkSpeed;
            }
        }

        public float JumpSpeed
        {
            get
            {
                return jumpSpeed;
            }
        }
    }
}
