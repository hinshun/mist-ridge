using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallCamera();
            InstallSettings();
        }

        private void InstallCamera()
        {
            Container.Bind<CameraOriginView>().ToSinglePrefab(settings.Prefab);
            Container.Bind<CameraAnchorView>().ToSinglePrefab(settings.Prefab);
            Container.Bind<CameraRigView>().ToSinglePrefab(settings.Prefab);
            Container.Bind<CameraView>().ToSinglePrefab(settings.Prefab);
            Container.Bind<Camera>().ToSinglePrefab(settings.Prefab);

            Container.Bind<CameraAnchorManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraAnchorManager>();

            Container.Bind<CameraManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraManager>();
        }

        private void InstallSettings()
        {
            Container.Bind<CameraAnchorManager.Settings>().ToSingleInstance(settings.Anchor);
            Container.Bind<CameraManager.Settings>().ToSingleInstance(settings.Camera);
        }

        [Serializable]
        public class Settings
        {
            public GameObject Prefab;
            public CameraAnchorManager.Settings Anchor;
            public CameraManager.Settings Camera;
        }
    }
}
