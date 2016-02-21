using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PlayerManager : IInitializable, ITickable
    {
        private readonly PlayerFacade.Factory playerFacadeFactory;
        private List<PlayerFacade> playerFacades;
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
            playerFacades = new List<PlayerFacade>();
            playerViews = new List<PlayerView>();
        }

        public void Tick()
        {
            foreach (PlayerFacade playerFacade in playerFacades)
            {
                playerFacade.Tick();
            }
        }

        public void SpawnPlayer(Input input)
        {
            PlayerFacade playerFacade = playerFacadeFactory.Create(input);
            playerFacades.Add(playerFacade);
            playerViews.Add(playerFacade.PlayerView);
        }
    }
}
