using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TutorialView : MonoView
    {
        private TutorialSignal.Trigger tutorialTrigger;
        private bool tutorialTriggered;

        [PostInject]
        public void Init(TutorialSignal.Trigger tutorialTrigger)
        {
            this.tutorialTrigger = tutorialTrigger;
        }

        public void OnTutorialTrigger(PlayerView playerView)
        {
            if (tutorialTriggered)
            {
                return;
            }

            tutorialTriggered = true;
            tutorialTrigger.Fire(TutorialType.Start, playerView);
        }

        private void Awake()
        {
            tutorialTriggered = false;
        }
    }
}
