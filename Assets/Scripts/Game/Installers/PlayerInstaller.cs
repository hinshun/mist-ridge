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
            InstallDeath();
            InstallRank();
            InstallSettings();
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerContainerView>().ToSinglePrefab(settings.player.playerContainerPrefab);

            Container.BindFacadeFactory<CharacterType, Input, PlayerFacade, PlayerFacadeFactory>(InstallPlayerFacade);

            Container.Bind<PlayerManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PlayerManager>();

            Container.Bind<PlayerPhysics>().ToSingleInstance(settings.player.playerPhysics);

            Container.BindSignal<CheckpointSignal>();
            Container.BindTrigger<CheckpointSignal.Trigger>();

            Container.BindSignal<CheckpointActionSignal>();
            Container.BindTrigger<CheckpointActionSignal.Trigger>();
        }

        private void InstallPlayerFacade(DiContainer subContainer, CharacterType characterType, Input input)
        {
            subContainer.BindInstance(characterType);
            subContainer.BindInstance(input);

            subContainer.Bind<Grounding>().ToSingle();

            subContainer.Bind<Player>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Player>();

            subContainer.Bind<PlayerController>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerController>();

            subContainer.Bind<PlayerInventory>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerInventory>();

            subContainer.BindSignal<ItemPickupSignal>();
            subContainer.BindTrigger<ItemPickupSignal.Trigger>();

            subContainer.BindSignal<ItemEffectSignal>();
            subContainer.BindTrigger<ItemEffectSignal.Trigger>();

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
            Container.Bind<SpawnManager>().ToSingle();
        }

        private void InstallDeath()
        {
            Container.Bind<DeathManager>().ToSingle();
            Container.BindAllInterfacesToSingle<DeathManager>();

            Container.Bind<MistContainerView>().ToSinglePrefab(settings.mistManagerSettings.mistContainerPrefab);
            Container.Bind<MistView>().ToSinglePrefab(settings.mistManagerSettings.mistContainerPrefab);

            Container.Bind<MistManager>().ToSingle();
            Container.BindAllInterfacesToSingle<MistManager>();
        }

        private void InstallRank()
        {
            Container.Bind<RankManager>().ToSingle();
            Container.BindAllInterfacesToSingle<RankManager>();
        }

        private void InstallSettings()
        {
            Container.Bind<PlayerManager.Settings>().ToSingleInstance(settings.playerManagerSettings);
            Container.Bind<DeathManager.Settings>().ToSingleInstance(settings.deathManagerSettings);
            Container.Bind<MistManager.Settings>().ToSingleInstance(settings.mistManagerSettings);

            Container.Bind<Player.Settings>().ToSingleInstance(settings.player.playerSettings);
            Container.Bind<PlayerController.Settings>().ToSingleInstance(settings.player.controllerSettings);
            Container.Bind<PlayerStateMachine.Settings>().ToSingleInstance(settings.player.stateMachineSettings);
            Container.Bind<PlayerFacadeFactory.Settings>().ToSingleInstance(settings.player.facadeFactorySettings);

            Container.Bind<Grounding.Settings>().ToSingleInstance(settings.player.collision.groundingSettings);

            Container.Bind<CameraAnchorManager.Settings>().ToSingleInstance(settings.camera.anchorSettings);
            Container.Bind<CameraManager.Settings>().ToSingleInstance(settings.camera.cameraSettings);
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings player;
            public CameraSettings camera;
            public PlayerManager.Settings playerManagerSettings;
            public DeathManager.Settings deathManagerSettings;
            public MistManager.Settings mistManagerSettings;

            [Serializable]
            public class PlayerSettings
            {
                public GameObject playerContainerPrefab;
                public PlayerPhysics playerPhysics;
                public CollisionSettings collision;
                public Player.Settings playerSettings;
                public PlayerController.Settings controllerSettings;
                public PlayerStateMachine.Settings stateMachineSettings;
                public PlayerFacadeFactory.Settings facadeFactorySettings;

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
        }
    }
}
