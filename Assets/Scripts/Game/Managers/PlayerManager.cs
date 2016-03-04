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
        private readonly PlayerFacade.Factory playerFacadeFactory;
        private Dictionary<Input, PlayerFacade> playerFacades;
        private Dictionary<PlayerView, Input> playerViewMapping;

        public PlayerManager(
                PlayerContainerView playerContainerView,
                SpawnManager spawnManager,
                PlayerFacade.Factory playerFacadeFactory)
        {
            this.playerContainerView = playerContainerView;
            this.spawnManager = spawnManager;
            this.playerFacadeFactory = playerFacadeFactory;
        }

        public List<Vector3> PlayerPositions
        {
            get
            {
                List<Vector3> playerPositions = new List<Vector3>();
                foreach (PlayerFacade playerFacade in playerFacades.Values)
                {
                    playerPositions.Add(playerFacade.Position);
                }

                return playerPositions;
            }
        }

        public List<Vector3> PlayerGroundingPositions
        {
            get
            {
                List<Vector3> playerPositions = new List<Vector3>();
                foreach (PlayerFacade playerFacade in playerFacades.Values)
                {
                    playerPositions.Add(playerFacade.GroundingPosition);
                }

                return playerPositions;
            }
        }

        public int PlayerCount
        {
            get
            {
                return playerFacades.Values.Count;
            }
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
                playerFacade.Tick();
            }
        }

        public PlayerFacade SpawnPlayer(Input input)
        {
            if (!playerFacades.ContainsKey(input))
            {
                PlayerFacade playerFacade = playerFacadeFactory.Create(input);
                playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(input.DeviceNum);
                playerFacade.Parent = playerContainerView.transform;

                playerFacades[input] = playerFacade;
                playerViewMapping[playerFacade.PlayerView] = input;

                return playerFacade;
            }

            return null;
        }
    }
}
