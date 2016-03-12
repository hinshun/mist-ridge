using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class GettingUpStateView : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerView>().CanControl = true;
        }
    }
}
