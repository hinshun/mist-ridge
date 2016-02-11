using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Zenject;

namespace MistRidge
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallInput();
            InstallUtility();
            InstallSettings();
        }

        void InstallInput()
        {
            Container.Bind<InputManager>().ToSingle();
            Container.BindAllInterfacesToSingle<InputManager>();

            Container.Bind<Input.Factory>().ToSingle();
        }

        void InstallUtility()
        {
            Container.Bind<SceneLoader>().ToSingle();
        }

        void InstallSettings()
        {
            Container.Bind<SceneLoader.Settings>().ToSingleInstance(settings.SceneLoader);
        }

        [Serializable]
        public class Settings
        {
            public SceneLoader.Settings SceneLoader;
        }
    }
}
