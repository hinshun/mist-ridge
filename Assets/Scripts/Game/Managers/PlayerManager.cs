using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly PlayerContainerView playerContainerView;
        private readonly SpawnManager spawnManager;
        private readonly DisplayManager displayManager;
        private readonly PlayerFacadeFactory playerFacadeFactory;
        private readonly PlayerControlSignal playerControlSignal;
        private Dictionary<Input, PlayerFacade> playerFacades;
        private Dictionary<PlayerView, Input> playerViewMapping;

        public PlayerManager(
                Settings settings,
                PlayerContainerView playerContainerView,
                SpawnManager spawnManager,
                DisplayManager displayManager,
                PlayerFacadeFactory playerFacadeFactory,
                PlayerControlSignal playerControlSignal)
        {
            this.settings = settings;
            this.playerContainerView = playerContainerView;
            this.spawnManager = spawnManager;
            this.displayManager = displayManager;
            this.playerFacadeFactory = playerFacadeFactory;
            this.playerControlSignal = playerControlSignal;
        }

        public PlayerFacade PlayerFacade(Input input)
        {
            return playerFacades[input];
        }

        public Input Input(PlayerView playerView)
        {
            return playerViewMapping[playerView];
        }

        public bool HasPlayerFacade(Input input)
        {
            return playerFacades.ContainsKey(input);
        }

        public void Initialize()
        {
            playerControlSignal.Event += OnPlayerControl;
            playerFacades = new Dictionary<Input, PlayerFacade>();
            playerViewMapping = new Dictionary<PlayerView, Input>();
        }

        public void Tick()
        {
            foreach (PlayerFacade playerFacade in playerFacades.Values)
            {
                if (playerFacade.IsAlive)
                {
                    playerFacade.Tick();
                }
            }
        }

        public PlayerFacade SpawnPlayer(Input input)
        {
            if (!playerFacades.ContainsKey(input))
            {
                CharacterType characterType = GetCharacterType(input);
                PlayerFacade playerFacade = playerFacadeFactory.Create(characterType, input);

                playerFacade.SetPlayerCircle(settings.playerCircles[input.DeviceNum]);

                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(input.DeviceNum);
                playerFacade.Parent = playerContainerView.transform;

                playerFacades[input] = playerFacade;
                playerViewMapping[playerFacade.PlayerView] = input;

                /* displayManager.Display(input.DeviceNum, playerFacade.CharacterType); */

                return playerFacade;
            }

            return null;
        }

        public CharacterType GetCharacterType(Input input)
        {
            if (input.DeviceNum % 2 == 0)
            {
                return CharacterType.Jack;
            }
            else
            {
                return CharacterType.Jill;
            }
        }

        public void ChangePlayerControl(bool control)
        {
            foreach (PlayerFacade playerFacade in playerFacades.Values)
            {
                playerFacade.Control = control;
            }
        }

        private void OnPlayerControl(PlayerView playerView, bool control)
        {
            Input input = Input(playerView);

            if (!HasPlayerFacade(input))
            {
                return;
            }

            PlayerFacade playerFacade = PlayerFacade(input);
            playerFacade.Control = control;
        }

        [Serializable]
        public class Settings
        {
            public List<Sprite> playerCircles;
        }
    }
}
