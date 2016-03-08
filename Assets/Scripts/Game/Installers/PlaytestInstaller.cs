using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlaytestInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallPlaytest();
            InstallSettings();
        }

        private void InstallPlaytest()
        {
            Container.Bind<PlaytestManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PlaytestManager>();

            Container.BindInstance(settings.playtestSpawn);
        }

        private void InstallSettings()
        {
        }

        [Serializable]
        public class Settings
        {
            public SpawnView playtestSpawn;
        }
    }
}
