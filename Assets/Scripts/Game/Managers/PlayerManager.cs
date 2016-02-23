using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerManager : IInitializable, ITickable
    {
        private readonly PlayerFacade.Factory playerFacadeFactory;
        private Dictionary<Input, PlayerFacade> playerFacades;

        public List<Vector3> PlayerPositions
        {
            get
            {
                List<Vector3> playerPositions = new List<Vector3>();
                foreach(PlayerFacade playerFacade in playerFacades.Values)
                {
                    playerPositions.Add(playerFacade.GroundingPosition);
                }

                return playerPositions;
            }
        }

        public PlayerManager(PlayerFacade.Factory playerFacadeFactory)
        {
            this.playerFacadeFactory = playerFacadeFactory;
        }

        public void Initialize()
        {
            playerFacades = new Dictionary<Input, PlayerFacade>();
        }

        public void Tick()
        {
            foreach (PlayerFacade playerFacade in playerFacades.Values)
            {
                playerFacade.Tick();
            }
        }

        public void SpawnPlayer(Input input)
        {
            if (!playerFacades.ContainsKey(input))
            {
                PlayerFacade playerFacade = playerFacadeFactory.Create(input);
                playerFacades[input] = playerFacade;
            }
        }
    }
}
