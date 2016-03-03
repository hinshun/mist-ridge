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
        }

        private void InstallSignals()
        {
            Container.BindSignal<MenuSignal>();
            Container.BindTrigger<MenuSignal.Trigger>();

            Container.BindSignal<GameStateSignal>();
            Container.BindTrigger<GameStateSignal.Trigger>();
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
            Container.Bind<UnityFixGI>().ToSinglePrefab(settings.unityFixGIPrefab);

            Container.Bind<Generator>().ToSingle();
            Container.BindAllInterfacesToSingle<Generator>();
        }

        private void InstallSettings()
        {
            Container.Bind<Generator.Settings>().ToSingleInstance(settings.generatorSettings);
            Container.Bind<PoolManager.Settings>().ToSingleInstance(settings.poolManagerSettings);

            Container.Bind<GameReadyState.Settings>().ToSingleInstance(settings.game.gameReadyStateSettings);
        }

        [Serializable]
        public class Settings
        {
            public GameObject inControlManagerPrefab;
            public GameObject unityFixGIPrefab;
            public Generator.Settings generatorSettings;
            public PoolManager.Settings poolManagerSettings;
            public GameSettings game;

            [Serializable]
            public class GameSettings
            {
                public GameReadyState.Settings gameReadyStateSettings;
            }
        }
    }
}
