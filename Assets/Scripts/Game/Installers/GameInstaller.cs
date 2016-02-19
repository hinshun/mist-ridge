using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallGame();
            InstallInput();
            InstallUtility();
            InstallSettings();
        }

        private void InstallGame()
        {
            Container.Bind<GameManager>().ToSingle();
            Container.BindAllInterfacesToSingle<GameManager>();

            Container.Bind<GameStateMachine>().ToSingle();
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
