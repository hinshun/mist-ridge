using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TimeMachineInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallTimeMachine();
            InstallSettings();
        }

        private void InstallTimeMachine()
        {
            Container.Bind<TimeMachineView>().ToSinglePrefab(settings.Prefab);
            Container.BindAllInterfacesToSingle<TimeMachine>();
        }

        private void InstallSettings()
        {

            Container.Bind<TimeMachine.Settings>().ToSingleInstance(settings.TimeMachine);
        }

        [Serializable]
        public class Settings
        {
            public GameObject Prefab;
            public TimeMachine.Settings TimeMachine;
        }
    }
}
