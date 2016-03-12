using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class HaltStateMachineView : StateMachineBehaviour
    {
        private PlayerView playerView;

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            playerView = animator.GetComponent<PlayerView>();
            playerView.IsDustTrailEmitting = true;
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            playerView.IsDustTrailEmitting = false;
        }
    }
}
