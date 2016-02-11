using UnityEngine;
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
            InstallGame();
            InstallSettings();
        }

        void InstallGame()
        {
            Container.Bind<InputManager>().ToSingle();
            Container.BindAllInterfacesToSingle<InputManager>();

            Container.Bind<Input.Factory>().ToSingle();
        }

        void InstallSettings()
        {
        }

        [Serializable]
        public class Settings
        {
        }
    }
}
