using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class StartDanceStateView : StateMachineBehaviour
    {
        private System.Random random;
        private Array danceStates;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            danceStates = Enum.GetValues(typeof(DanceState));
            random = new System.Random();

            DanceState danceState = (DanceState)danceStates.GetValue(random.Next(danceStates.Length));

            switch (danceState)
            {
                case DanceState.GangnamStyle:
                    animator.SetTrigger("StartGangnamStyle");
                    break;

                case DanceState.HipHop:
                    animator.SetTrigger("StartHipHop");
                    break;

                case DanceState.Shuffling:
                    animator.SetTrigger("StartShuffling");
                    break;
            }
        }

        private enum DanceState
        {
            GangnamStyle,
            HipHop,
            Shuffling,
        }
    }
}
