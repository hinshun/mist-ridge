using UnityEngine;
using System;
using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class DisplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Settings settings;

        public override void InstallBindings()
        {
            InstallDisplay();
            InstallSignals();
            InstallSettings();
        }

        private void InstallDisplay()
        {
            Container.Bind<DisplayManager>().ToSingle();
            Container.BindAllInterfacesToSingle<DisplayManager>();

            Container.Bind<CinematicManager>().ToSingle();
            Container.BindAllInterfacesToSingle<CinematicManager>();

            Container.Bind<DialogueManager>().ToSingle();
            Container.BindAllInterfacesToSingle<DialogueManager>();

            Container.Bind<GameDisplayView>().ToSinglePrefab(settings.gameDisplayPrefab);
            Container.Bind<CharacterSelectDisplayView>().ToSinglePrefab(settings.characterSelectDisplayPrefab);
            Container.Bind<CinematicDisplayView>().ToSinglePrefab(settings.cinematicDisplayPrefab);
            Container.Bind<DialogueDisplayView>().ToSinglePrefab(settings.dialogueDisplayPrefab);
            Container.Bind<ReadySetGoDisplayView>().ToSinglePrefab(settings.readySetGoDisplayPrefab);
            Container.Bind<ScoreDisplayView>().ToSinglePrefab(settings.scoreDisplayPrefab);
        }

        private void InstallSignals()
        {
            Container.BindSignal<DialogueSignal>();
            Container.BindTrigger<DialogueSignal.Trigger>();

            Container.BindSignal<DialogueStateSignal>();
            Container.BindTrigger<DialogueStateSignal.Trigger>();
        }

        private void InstallSettings()
        {
            Container.Bind<DisplayManager.Settings>().ToSingleInstance(settings.displayManagerSettings);
            Container.Bind<DialogueManager.Settings>().ToSingleInstance(settings.dialogueManagerSettings);
        }

        [Serializable]
        public class Settings
        {
            public GameObject gameDisplayPrefab;
            public GameObject characterSelectDisplayPrefab;
            public GameObject cinematicDisplayPrefab;
            public GameObject dialogueDisplayPrefab;
            public GameObject readySetGoDisplayPrefab;
            public GameObject scoreDisplayPrefab;
            public DisplayManager.Settings displayManagerSettings;
            public DialogueManager.Settings dialogueManagerSettings;
        }
    }
}
