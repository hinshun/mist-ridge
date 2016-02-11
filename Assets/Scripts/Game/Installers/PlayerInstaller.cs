using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings;

        public override void InstallBindings()
        {
            InstallPlayer();
            InstallSettings();
        }

        void InstallPlayer()
        {
            Container.BindFacadeFactory<Input, PlayerFacade, PlayerFacade.Factory>(InstallPlayerFacade);
        }

        void InstallPlayerFacade(DiContainer subContainer, Input input)
        {
            subContainer.BindInstance(input);
            subContainer.Bind<PlayerView>().ToSinglePrefab(settings.Player.Prefab);
        }

        void InstallSettings()
        {
        }

        [Serializable]
        public class Settings
        {
            public PlayerSettings Player;

            [Serializable]
            public class PlayerSettings
            {
                public GameObject Prefab;
            }
        }
    }
}
