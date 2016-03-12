using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class IdleStateView : StateMachineBehaviour
    {
        [SerializeField]
        private float minChangeTime;

        [SerializeField]
        private float maxChangeTime;

        private System.Random random;
        private float timer;
        private float nextChange;
        private Array idleStates;

        private PlayerView playerView;
        private bool dustTrailEmitting;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float horizontal = animator.GetFloat("Horizontal");
            if (horizontal > 0.1f || horizontal < 0.1f)
            {
                timer = Time.time;
            }

            if (Time.time - timer >= nextChange)
            {
                timer = Time.time;
                IdleState idleState = (IdleState)idleStates.GetValue(random.Next(idleStates.Length));

                switch (idleState)
                {
                    case IdleState.Bored:
                        animator.SetTrigger("BeBored");
                        break;

                    case IdleState.Bashful:
                        animator.SetTrigger("BeBashful");
                        break;
                }
            }

            if (dustTrailEmitting)
            {
                if (playerView.IsDustTrailEmitting)
                {
                    playerView.IsDustTrailEmitting = false;
                }

                dustTrailEmitting = false;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerView = animator.GetComponent<PlayerView>();
            idleStates = Enum.GetValues(typeof(IdleState));
            random = new System.Random();

            timer = Time.time;
            nextChange = UnityEngine.Random.Range(minChangeTime, maxChangeTime);

            playerView.IsDustTrailEmitting = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            dustTrailEmitting = true;
        }

        private enum IdleState
        {
            Bored,
            Bashful,
        }
    }
}
