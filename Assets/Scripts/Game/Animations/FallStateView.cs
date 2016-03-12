using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class FallStateView : StateMachineBehaviour
    {
        private PlayerView playerView;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerView = animator.GetComponent<PlayerView>();
            playerView.IsDustTrailEmitting = false;
        }
    }
}
