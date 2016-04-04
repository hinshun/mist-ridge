using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TutorialManager : IInitializable
    {
        private readonly Settings settings;
        private readonly TutorialSignal tutorialSignal;
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;
        private readonly DeathManager deathManager;
        private readonly CinematicManager cinematicManager;
        private readonly CameraManager cameraManager;
        private readonly CameraRigManager cameraRigManager;

        public TutorialManager(
                Settings settings,
                TutorialSignal tutorialSignal,
                PlayerManager playerManager,
                DisplayManager displayManager,
                DeathManager deathManager,
                CameraManager cameraManager,
                CameraRigManager cameraRigManager,
                CinematicManager cinematicManager)
        {
            this.settings = settings;
            this.tutorialSignal = tutorialSignal;
            this.playerManager = playerManager;
            this.displayManager = displayManager;
            this.deathManager = deathManager;
            this.cameraManager = cameraManager;
            this.cameraRigManager = cameraRigManager;
            this.cinematicManager = cinematicManager;
        }

        public void Initialize()
        {
            tutorialSignal.Event += OnTutorialEvent;
        }

        private void OnTutorialEvent(TutorialType tutorialType)
        {
            switch(tutorialType)
            {
                case TutorialType.Start:
                    StartTutorial();
                    break;

                case TutorialType.End:
                    EndTutorial();
                    break;
            }
        }

        private void StartTutorial()
        {
            playerManager.ChangePlayerControl(false);
            displayManager.UpdateCinematic(true);
            deathManager.IsActive = false;
            cameraManager.ZoomOverride = settings.zoomOverride;
            cameraManager.ZoomOverrideEnabled = true;
            cameraRigManager.RigPosition = settings.rigPosition;
            cinematicManager.CinematicType = CinematicType.StartingZone;

            TurnipView turnipView = cinematicManager.StartingZoneView.TurnipView;
            turnipView.Alert();
        }

        private void EndTutorial()
        {
            playerManager.ChangePlayerControl(true);
            displayManager.UpdateCinematic(false);
            deathManager.IsActive = true;
            cameraManager.ZoomOverrideEnabled = false;
            cameraRigManager.ResetRig();
            cinematicManager.CinematicType = CinematicType.None;
        }

        [Serializable]
        public class Settings
        {
            public float zoomOverride;
            public Vector3 rigPosition;
        }
    }
}
