using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class WalkStateView : StateMachineBehaviour
    {
        [SerializeField]
        private float dustDuration;

        [SerializeField]
        private float dustSpeedThreshold;

        private PlayerView playerView;
        private bool emitting;
        private bool hitThreshold;
        private float timer;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (emitting && Time.time - timer >= dustDuration)
            {
                emitting = false;
                playerView.IsDustTrailEmitting = false;
            }

            EmitDustTrail(animator);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerView = animator.GetComponent<PlayerView>();
            EmitDustTrail(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            emitting = false;
            playerView.IsDustTrailEmitting = false;
        }

        private void EmitDustTrail(Animator animator)
        {
            if (animator.GetFloat("Speed") > dustSpeedThreshold)
            {
                if (hitThreshold)
                {
                    return;
                }

                emitting = true;
                hitThreshold = true;
                timer = Time.time;
                playerView.IsDustTrailEmitting = true;
            }
            else
            {
                hitThreshold = false;
            }
        }
    }
}
