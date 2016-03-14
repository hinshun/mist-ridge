using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class PlayerFacadeFactory : FacadeFactory<CharacterType, Input, PlayerFacade>
    {
        private readonly Settings settings;

        public PlayerFacadeFactory(Settings settings)
        {
            this.settings = settings;
        }

        public override PlayerFacade Create(CharacterType characterType, Input input)
        {
            DiContainer subContainer = CreateSubContainer(characterType, input);

            GameObject playerViewPrefab = null;
            switch (characterType)
            {
                case CharacterType.Jack:
                    playerViewPrefab = settings.jackPrefab;
                    break;

                case CharacterType.Jill:
                    playerViewPrefab = settings.jillPrefab;
                    break;
            }
            subContainer.Bind<PlayerView>().ToSinglePrefab(playerViewPrefab);

            PlayerFacade playerFacade = subContainer.Resolve<PlayerFacade>();
            playerFacade.Initialize();

            return playerFacade;
        }

        [Serializable]
        public class Settings
        {
            public GameObject jackPrefab;
            public GameObject jillPrefab;
        }
    }
}
