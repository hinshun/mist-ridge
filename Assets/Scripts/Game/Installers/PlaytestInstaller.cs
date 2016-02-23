using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlaytestInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallPlaytest();
            InstallSettings();
        }

        private void InstallPlaytest()
        {
            Container.Bind<PlaytestManager>().ToSingle();
            Container.BindAllInterfacesToSingle<PlaytestManager>();
        }

        private void InstallSettings()
        {
        }
    }
}
