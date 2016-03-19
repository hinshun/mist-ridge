using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class BillboardView : MonoView
    {
        private CameraManager cameraManager;

        [PostInject]
        public void Init(CameraManager cameraManager)
        {
            this.cameraManager = cameraManager;
        }

        public void Update()
        {
            Camera camera = cameraManager.CurrentCamera;

            transform.LookAt(
                Position + camera.transform.rotation * Vector3.forward,
                camera.transform.rotation * Vector3.up
            );
        }
    }
}
