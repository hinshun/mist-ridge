using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class JumpStateMachineView : StateMachineBehaviour
    {
        private PlayerView playerView;

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (playerView == null)
            {
                playerView = animator.GetComponent<PlayerView>();
            }

            playerView.IsDustTrailEmitting = false;
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (playerView == null)
            {
                playerView = animator.GetComponent<PlayerView>();
            }

            playerView.CanJump = true;
        }
    }
}
