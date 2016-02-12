using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallPlayer();
            InstallCamera();
            InstallSettings();
        }

        void InstallPlayer()
        {
            Container.BindFacadeFactory<Input, PlayerFacade, PlayerFacade.Factory>(InstallPlayerFacade);

            Container.Bind<PlayerManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PlayerManager>();

            Container.Bind<PlayerInputHandler>().ToSingle();
            Container.BindAllInterfacesToSingle<PlayerInputHandler>();
        }

        void InstallPlayerFacade(DiContainer subContainer, Input input)
        {
            subContainer.BindInstance(input);
            subContainer.Bind<PlayerView>().ToSinglePrefab(settings.Player.Prefab);

            subContainer.Bind<PlayerController>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerController>();
        }

        void InstallCamera()
        {
            Container.Bind<CameraOriginView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraAnchorView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraRigView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraView>().ToSinglePrefab(settings.Camera.Prefab);

            Container.Bind<CameraAnchorManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraAnchorManager>();

            Container.Bind<CameraManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraManager>();
        }

        void InstallSettings()
        {
            Container.Bind<CameraAnchorManager.Settings>().ToSingleInstance(settings.Camera.Anchor);
            Container.Bind<CameraManager.Settings>().ToSingleInstance(settings.Camera.Camera);
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings Player;
            public CameraSettings Camera;

            [Serializable]
            public class PlayerSettings
            {
                public GameObject Prefab;
            }

            [Serializable]
            public class CameraSettings
            {
                public GameObject Prefab;
                public CameraAnchorManager.Settings Anchor;
                public CameraManager.Settings Camera;
            }
        }
    }
}
