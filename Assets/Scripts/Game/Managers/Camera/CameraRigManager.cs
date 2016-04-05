using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class CameraRigManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly CameraView cameraView;
        private readonly CameraRigView cameraRigView;

        private Vector3 initialRigPosition;
        private Vector3 rigPosition;

        public CameraRigManager(
                Settings settings,
                CameraView cameraView,
                CameraRigView cameraRigView)
        {
            this.settings = settings;
            this.cameraView = cameraView;
            this.cameraRigView = cameraRigView;
        }

        public Vector3 RigPosition
        {
            get
            {
                return rigPosition;
            }
            set
            {
                rigPosition = value;
            }
        }

        public void Initialize()
        {
            initialRigPosition = cameraRigView.LocalPosition;
            ResetVariables();
        }

        public void ResetVariables()
        {
            rigPosition = initialRigPosition;
        }

        public void Tick()
        {
            if (!cameraView.IsActive)
            {
                return;
            }

            cameraRigView.LocalPosition = Vector3.Lerp(
                cameraRigView.LocalPosition,
                rigPosition,
                settings.translationSpeed * Time.deltaTime
            );
        }


        [Serializable]
        public class Settings
        {
            public float translationSpeed;
        }
    }
}
