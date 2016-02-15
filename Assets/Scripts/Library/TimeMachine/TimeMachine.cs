using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TimeMachine : IInitializable
    {
        private readonly Settings settings;
        private readonly TimeMachineView timeMachineView;

        private float timeScale;

        public TimeMachine(
                Settings settings,
                TimeMachineView timeMachineView)
        {
            this.settings = settings;
            this.timeMachineView = timeMachineView;
        }

        public void Initialize()
        {
            timeScale = settings.initialTimeScale;
            timeMachineView.GUI += OnGUI;
        }

        private void OnGUI()
        {
            GUI.Box(
                new Rect(10, 10, 200, 50),
                "Time Machine"
            );

            timeScale = GUI.HorizontalSlider(
                new Rect(20, 40, 180, 20),
                timeScale,
                0f,
                1f
            );

            Time.timeScale = timeScale;
        }


        [Serializable]
        public class Settings
        {
            public float initialTimeScale;
        }
    }
}
