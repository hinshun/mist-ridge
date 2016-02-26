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
            Container.Bind<TimeMachineView>().ToSinglePrefab(settings.timeMachinePrefab);
            Container.BindAllInterfacesToSingle<TimeMachine>();
        }

        private void InstallSettings()
        {

            Container.Bind<TimeMachine.Settings>().ToSingleInstance(settings.timeMachineSettings);
        }

        [Serializable]
        public class Settings
        {
            public GameObject timeMachinePrefab;
            public TimeMachine.Settings timeMachineSettings;
        }
    }
}
