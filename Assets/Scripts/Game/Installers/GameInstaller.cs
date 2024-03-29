using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InitExecutionOrder();
            InstallGame();
            InstallSignals();
            InstallInput();
            InstallObjectPool();
            InstallUtility();
            InstallSettings();
        }

        private void InitExecutionOrder()
        {
            Container.Install<ExecutionOrderInstaller>(
                new List<Type>()
                {
                    typeof(DisplayManager),
                    typeof(InputManager),
                    typeof(GameManager),
                }
            );
        }

        private void InstallGame()
        {
            Container.Bind<GameManager>().ToSingle();
            Container.BindAllInterfacesToSingle<GameManager>();

            Container.Bind<GameStateMachine>().ToSingle();
            Container.Bind<GameStateFactory>().ToSingle();

            Container.Bind<AetherManager>().ToSingle();
            Container.BindAllInterfacesToSingle<AetherManager>();
        }

        private void InstallSignals()
        {
            Container.BindSignal<ItemGainSignal>();
            Container.BindTrigger<ItemGainSignal.Trigger>();

            Container.BindSignal<AetherGainSignal>();
            Container.BindTrigger<AetherGainSignal.Trigger>();

            Container.BindSignal<RespawnSignal>();
            Container.BindTrigger<RespawnSignal.Trigger>();

            Container.BindSignal<MenuSignal>();
            Container.BindTrigger<MenuSignal.Trigger>();

            Container.BindSignal<GameStateSignal>();
            Container.BindTrigger<GameStateSignal.Trigger>();

            Container.BindSignal<SceneLoadSignal>();
            Container.BindTrigger<SceneLoadSignal.Trigger>();
        }

        private void InstallInput()
        {
            Container.Bind<InControl.InControlManager>().ToSinglePrefab(settings.inControlManagerPrefab);

            Container.Bind<InputManager>().ToSingle();
            Container.BindAllInterfacesToSingle<InputManager>();

            Container.Bind<Input.Factory>().ToSingle();
        }

        private void InstallObjectPool()
        {
            Container.Bind<PoolManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PoolManager>();

            Container.Bind<Pool.Factory>().ToSingle();

            Container.Bind<ObjectPoolView>().ToSinglePrefab(settings.poolManagerSettings.objectPoolPrefab);
            Container.Bind<PoolView>().ToTransientPrefab(settings.poolManagerSettings.poolPrefab);
        }

        private void InstallUtility()
        {
            Container.Bind<SceneLoader>().ToSingle();
            Container.BindAllInterfacesToSingle<SceneLoader>();
            Container.Bind<SceneLoaderView>().ToSinglePrefab(settings.sceneLoaderPrefab);

            Container.Bind<UnityFixGI>().ToSinglePrefab(settings.unityFixGIPrefab);

            Container.Bind<Generator>().ToSingle();
            Container.BindAllInterfacesToSingle<Generator>();
        }

        private void InstallSettings()
        {
            Container.Bind<Generator.Settings>().ToSingleInstance(settings.generatorSettings);
            Container.Bind<PoolManager.Settings>().ToSingleInstance(settings.poolManagerSettings);
            Container.Bind<AetherManager.Settings>().ToSingleInstance(settings.aetherManagerSettings);

            Container.Bind<GameReadyState.Settings>().ToSingleInstance(settings.game.gameReadyStateSettings);
            Container.Bind<GameEndState.Settings>().ToSingleInstance(settings.game.gameEndStateSettings);
        }

        [Serializable]
        public class Settings
        {
            public GameObject inControlManagerPrefab;
            public GameObject unityFixGIPrefab;
            public GameObject sceneLoaderPrefab;
            public Generator.Settings generatorSettings;
            public PoolManager.Settings poolManagerSettings;
            public AetherManager.Settings aetherManagerSettings;
            public GameSettings game;

            [Serializable]
            public class GameSettings
            {
                public GameReadyState.Settings gameReadyStateSettings;
                public GameEndState.Settings gameEndStateSettings;
            }
        }
    }
}
