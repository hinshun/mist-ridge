using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class DisplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallDisplay();
            InstallSettings();
        }

        private void InstallDisplay()
        {
            Container.Bind<DisplayManager>().ToSingle();
            Container.BindAllInterfacesToSingle<DisplayManager>();

            Container.Bind<GameDisplayView>().ToSinglePrefab(settings.gameDisplayPrefab);
        }

        private void InstallSettings()
        {
        }

        [Serializable]
        public class Settings
        {
            public GameObject gameDisplayPrefab;
        }
    }
}
