using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallLevel();
            InstallSettings();
        }

        private void InstallLevel()
        {
        }

        private void InstallSettings()
        {
            Container.Bind<ChunkManager.Settings>().ToSingleInstance(settings.ChunkManager);

        }

        [Serializable]
        public class Settings
        {
            public ChunkManager.Settings ChunkManager;
        }
    }
}
