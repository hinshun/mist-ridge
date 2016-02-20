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
            Container.Bind<InControl.InControlManager>().ToSinglePrefab(settings.InControlManagerPrefab);

            Container.Bind<InputManager>().ToSingle();
            Container.BindAllInterfacesToSingle<InputManager>();

            Container.Bind<Input.Factory>().ToSingle();
        }

        private void InstallUtility()
        {
            Container.Bind<SceneLoader>().ToSingle();
            Container.Bind<UnityFixGI>().ToSinglePrefab(settings.UnityFixGIPrefab);
        }

        private void InstallSettings()
        {
            Container.Bind<SceneLoader.Settings>().ToSingleInstance(settings.SceneLoader);
        }

        [Serializable]
        public class Settings
        {
            public SceneLoader.Settings SceneLoader;
            public GameObject InControlManagerPrefab;
            public GameObject UnityFixGIPrefab;
        }
    }
}
