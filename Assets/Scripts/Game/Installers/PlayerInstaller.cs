using UnityEngine;
using System;
using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallPlayer();
            InstallCollision();
            InstallCamera();
            InstallSpawn();
            InstallSettings();
        }

        private void InstallPlayer()
        {
            Container.BindFacadeFactory<Input, PlayerFacade, PlayerFacade.Factory>(InstallPlayerFacade);

            Container.Bind<PlayerManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PlayerManager>();

            Container.Bind<PlayerPhysics>().ToSingleInstance(settings.player.playerPhysics);
        }

        private void InstallPlayerFacade(DiContainer subContainer, Input input)
        {
            subContainer.BindInstance(input);
            subContainer.Bind<Grounding>().ToSingle();

            subContainer.Bind<Player>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Player>();

            subContainer.Bind<PlayerView>().ToSinglePrefab(settings.player.playerPrefab);

            subContainer.Bind<PlayerController>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerController>();

            subContainer.Bind<PlayerInventory>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerInventory>();

            subContainer.BindSignal<ItemPickupSignal>();
            subContainer.BindTrigger<ItemPickupSignal.Trigger>();

            subContainer.Bind<PlayerStateMachine>().ToSingle();
            subContainer.Bind<PlayerStateFactory>().ToSingle();
        }

        private void InstallCollision()
        {
            Container.Bind<Collidable>().ToSinglePrefab(settings.player.collision.defaultCollidablePrefab);

            Container.Bind<CollisionSphere.Factory>().ToSingle();

        }

        private void InstallCamera()
        {
            Container.Bind<CameraOriginView>().ToSinglePrefab(settings.camera.cameraPrefab);
            Container.Bind<CameraAnchorView>().ToSinglePrefab(settings.camera.cameraPrefab);
            Container.Bind<CameraRigView>().ToSinglePrefab(settings.camera.cameraPrefab);
            Container.Bind<CameraView>().ToSinglePrefab(settings.camera.cameraPrefab);
            Container.Bind<Camera>().ToSinglePrefab(settings.camera.cameraPrefab);

            Container.Bind<CameraAnchorManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraAnchorManager>();

            Container.Bind<CameraManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraManager>();
        }

        private void InstallSpawn()
        {
            Container.Bind<SpawnView>().ToTransientPrefab(settings.spawn.spawnPrefab);
            Container.Bind<SpawnManager>().ToSingle();
            Container.Bind<Spawn.Factory>().ToSingle();
        }

        private void InstallSettings()
        {
            Container.Bind<Player.Settings>().ToSingleInstance(settings.player.playerSettings);
            Container.Bind<PlayerController.Settings>().ToSingleInstance(settings.player.controllerSettings);
            Container.Bind<PlayerStateMachine.Settings>().ToSingleInstance(settings.player.stateMachineSettings);

            Container.Bind<Grounding.Settings>().ToSingleInstance(settings.player.collision.groundingSettings);

            Container.Bind<CameraAnchorManager.Settings>().ToSingleInstance(settings.camera.anchorSettings);
            Container.Bind<CameraManager.Settings>().ToSingleInstance(settings.camera.cameraSettings);
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings player;
            public CameraSettings camera;
            public SpawnSettings spawn;

            [Serializable]
            public class PlayerSettings
            {
                public CollisionSettings collision;

                public GameObject playerPrefab;
                public PlayerPhysics playerPhysics;
                public Player.Settings playerSettings;
                public PlayerController.Settings controllerSettings;
                public PlayerStateMachine.Settings stateMachineSettings;

                [Serializable]
                public class CollisionSettings
                {
                    public GameObject defaultCollidablePrefab;
                    public Grounding.Settings groundingSettings;
                }
            }

            [Serializable]
            public class CameraSettings
            {
                public GameObject cameraPrefab;
                public CameraAnchorManager.Settings anchorSettings;
                public CameraManager.Settings cameraSettings;
            }

            [Serializable]
            public class SpawnSettings
            {
                public GameObject spawnPrefab;
            }
        }
    }
}
