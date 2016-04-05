using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerManager : IInitializable, IDisposable, ITickable
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
            ResetVariables();
        }

        public void Dispose()
        {
            playerControlSignal.Event -= OnPlayerControl;
        }

        public void ResetVariables()
        {
            if (playerFacades != null)
            {
                foreach (PlayerFacade playerFacade in playerFacades.Values)
                {
                    playerFacade.Dispose();
                }
            }

            foreach (Transform child in playerContainerView.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

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

        public PlayerFacade SpawnPlayer(Input input, CharacterType characterType)
        {
            if (!playerFacades.ContainsKey(input))
            {
                PlayerFacade playerFacade = playerFacadeFactory.Create(characterType, input);

                playerFacade.SetPlayerCircle(settings.playerCircles[input.DeviceNum]);

                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(input.DeviceNum);
                playerFacade.Parent = playerContainerView.transform;

                playerFacades[input] = playerFacade;
                playerViewMapping[playerFacade.PlayerView] = input;

                return playerFacade;
            }

            return null;
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
