using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerManager : IInitializable, ITickable
    {
        private readonly PlayerContainerView playerContainerView;
        private readonly SpawnManager spawnManager;
        private readonly DisplayManager displayManager;
        private readonly PlayerFacadeFactory playerFacadeFactory;
        private Dictionary<Input, PlayerFacade> playerFacades;
        private Dictionary<PlayerView, Input> playerViewMapping;

        public PlayerManager(
                PlayerContainerView playerContainerView,
                SpawnManager spawnManager,
                DisplayManager displayManager,
                PlayerFacadeFactory playerFacadeFactory)
        {
            this.playerContainerView = playerContainerView;
            this.spawnManager = spawnManager;
            this.displayManager = displayManager;
            this.playerFacadeFactory = playerFacadeFactory;
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

                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(input.DeviceNum);
                playerFacade.Parent = playerContainerView.transform;

                playerFacades[input] = playerFacade;
                playerViewMapping[playerFacade.PlayerView] = input;

                displayManager.Display(input, playerFacade.CharacterType);

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
    }
}
