using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class RandomIdles : StateMachineBehaviour
    {
        [SerializeField]
        private float minChangeTime;

        [SerializeField]
        private float maxChangeTime;

        private System.Random random;
        private float timer;
        private float nextChange;
        private Array idleStates;

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (Time.time - timer >= nextChange)
            {
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
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Array idleStates = Enum.GetValues(typeof(IdleState));
            random = new System.Random();

            timer = Time.time;
            nextChange = UnityEngine.Random.Range(minChangeTime, maxChangeTime);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        }

        private enum IdleState
        {
            Bored,
            Bashful,
        }
    }
}
