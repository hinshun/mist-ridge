using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class IdleStateMachineView : StateMachineBehaviour
    {
        private PlayerView playerView;

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            playerView = animator.GetComponent<PlayerView>();
            playerView.PlayerAllowJump();
        }
    }
}
