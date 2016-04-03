using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class ReadyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallReady();
        }

        private void InstallReady()
        {
            Container.Bind<ReadyManager>().ToSingle();
            Container.BindAllInterfacesToSingle<ReadyManager>();
        }
    }
}
