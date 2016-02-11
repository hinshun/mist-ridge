using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class StartMenuInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallStartMenu();
            InstallSettings();
        }

        void InstallStartMenu()
        {
            Container.Bind<StartMenuManager>().ToSingle();
            Container.BindAllInterfacesToSingle<StartMenuManager>();

            Container.Bind<StartMenuInputHandler>().ToSingle();
            Container.BindAllInterfacesToSingle<StartMenuInputHandler>();
        }

        void InstallSettings()
        {
            Container.Bind<StartMenuManager.Settings>().ToSingleInstance(settings.StartMenu);
        }

        [Serializable]
        public class Settings
        {
            public StartMenuManager.Settings StartMenu;
        }
    }
}
