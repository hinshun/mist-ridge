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

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
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
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            idleStates = Enum.GetValues(typeof(IdleState));
            random = new System.Random();

            timer = Time.time;
            nextChange = UnityEngine.Random.Range(minChangeTime, maxChangeTime);
        }

        private enum IdleState
        {
            Bored,
            Bashful,
        }
    }
}
