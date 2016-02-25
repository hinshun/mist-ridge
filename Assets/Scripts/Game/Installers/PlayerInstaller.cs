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

            Container.Bind<PlayerStateFactory>().ToSingle();
        }

        private void InstallPlayerFacade(DiContainer subContainer, Input input)
        {
            subContainer.BindInstance(input);
            subContainer.Bind<Grounding>().ToSingle();

            subContainer.Bind<Player>().ToSingle();
            subContainer.BindAllInterfacesToSingle<Player>();

            subContainer.Bind<PlayerView>().ToSinglePrefab(settings.Player.Prefab);

            subContainer.Bind<PlayerController>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerController>();

            subContainer.Bind<PlayerInventory>().ToSingle();
            subContainer.BindAllInterfacesToSingle<PlayerInventory>();

            subContainer.BindSignal<ItemPickupSignal>();
            subContainer.BindTrigger<ItemPickupSignal.Trigger>();

            subContainer.Bind<PlayerStateMachine>().ToSingle();
        }

        private void InstallCollision()
        {
            Container.Bind<Collidable>().ToSinglePrefab(settings.Player.Collision.DefaultCollidablePrefab);

            Container.Bind<CollisionSphere.Factory>().ToSingle();

        }

        private void InstallCamera()
        {
            Container.Bind<CameraOriginView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraAnchorView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraRigView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<CameraView>().ToSinglePrefab(settings.Camera.Prefab);
            Container.Bind<Camera>().ToSinglePrefab(settings.Camera.Prefab);

            Container.Bind<CameraAnchorManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraAnchorManager>();

            Container.Bind<CameraManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CameraManager>();
        }

        private void InstallSpawn()
        {
            Container.Bind<SpawnView>().ToTransientPrefab(settings.spawn.prefab);
            Container.Bind<SpawnManager>().ToSingle();
            Container.Bind<Spawn.Factory>().ToSingle();
        }

        private void InstallSettings()
        {
            Container.Bind<Player.Settings>().ToSingleInstance(settings.Player.Player);
            Container.Bind<PlayerController.Settings>().ToSingleInstance(settings.Player.Controller);
            Container.Bind<PlayerStateMachine.Settings>().ToSingleInstance(settings.Player.StateMachine);

            Container.Bind<PlayerIdleState.Settings>().ToSingleInstance(settings.Player.IdleState);
            Container.Bind<PlayerWalkState.Settings>().ToSingleInstance(settings.Player.WalkState);
            Container.Bind<PlayerJumpState.Settings>().ToSingleInstance(settings.Player.JumpState);
            Container.Bind<PlayerFallState.Settings>().ToSingleInstance(settings.Player.FallState);

            Container.Bind<Grounding.Settings>().ToSingleInstance(settings.Player.Collision.Grounding);

            Container.Bind<CameraAnchorManager.Settings>().ToSingleInstance(settings.Camera.Anchor);
            Container.Bind<CameraManager.Settings>().ToSingleInstance(settings.Camera.Camera);
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings Player;
            public CameraSettings Camera;
            public SpawnSettings spawn;

            [Serializable]
            public class PlayerSettings
            {
                public GameObject Prefab;
                public Player.Settings Player;
                public CollisionSettings Collision;
                public PlayerController.Settings Controller;
                public PlayerStateMachine.Settings StateMachine;
                public PlayerIdleState.Settings IdleState;
                public PlayerWalkState.Settings WalkState;
                public PlayerJumpState.Settings JumpState;
                public PlayerFallState.Settings FallState;

                [Serializable]
                public class CollisionSettings
                {
                    public GameObject DefaultCollidablePrefab;
                    public Grounding.Settings Grounding;
                }
            }

            [Serializable]
            public class CameraSettings
            {
                public GameObject Prefab;
                public CameraAnchorManager.Settings Anchor;
                public CameraManager.Settings Camera;
            }

            [Serializable]
            public class SpawnSettings
            {
                public GameObject prefab;
            }
        }
    }
}
