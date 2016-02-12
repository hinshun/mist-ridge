using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GlobalInstaller : MonoInstaller
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
            Container.Bind<InControl.InControlManager>().ToSinglePrefab(settings.InControlManagerPrefab);

            Container.Bind<InputManager>().ToSingle();
            Container.BindAllInterfacesToSingle<InputManager>();

            Container.Bind<Input.Factory>().ToSingle();
        }

        void InstallUtility()
        {
            Container.Bind<SceneLoader>().ToSingle();
            Container.Bind<UnityFixGI>().ToSinglePrefab(settings.UnityFixGIPrefab);
        }

        void InstallSettings()
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
