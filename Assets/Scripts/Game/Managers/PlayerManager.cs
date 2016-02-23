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
        private List<PlayerView> playerViews;

        public List<PlayerView> PlayerViews
        {
            get
            {
                return playerViews;
            }
        }

        public PlayerManager(PlayerFacade.Factory playerFacadeFactory)
        {
            this.playerFacadeFactory = playerFacadeFactory;
        }

        public void Initialize()
        {
            playerFacades = new Dictionary<Input, PlayerFacade>();
            playerViews = new List<PlayerView>();
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
                playerViews.Add(playerFacade.PlayerView);
            }
        }
    }
}
