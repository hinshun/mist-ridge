using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ReadyInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallReady();
            InstallSettings();
        }

        private void InstallReady()
        {
            Container.Bind<ReadyManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ReadyManager>();

            Container.BindInstance(settings.spawnView);
        }

        private void InstallSettings()
        {
            Container.Bind<ReadyManager.Settings>().ToInstance(settings.readyManagerSettings);
        }

        [Serializable]
        public class Settings
        {
            public SpawnView spawnView;
            public ReadyManager.Settings readyManagerSettings;
        }
    }
}
