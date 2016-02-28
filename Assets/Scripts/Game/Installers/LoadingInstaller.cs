using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LoadingInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallLoading();
            InstallSettings();
        }

        private void InstallLoading()
        {
            Container.Bind<LoadingManager>().ToSingle();
            Container.BindAllInterfacesToSingle<LoadingManager>();
        }

        private void InstallSettings()
        {
            Container.Bind<LoadingManager.Settings>().ToSingleInstance(settings.loadingSettings);
        }

        [Serializable]
        public class Settings
        {
            public LoadingManager.Settings loadingSettings;
        }
    }
}
