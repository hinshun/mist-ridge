using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class RankManager : ITickable
    {
        private readonly DisplayManager displayManager;
        private readonly PlayerManager playerManager;
        private readonly DeathManager deathManager;

        private bool isActive;

        public RankManager(
                DisplayManager displayManager,
                PlayerManager playerManager,
                DeathManager deathManager)
        {
            this.displayManager = displayManager;
            this.playerManager = playerManager;
            this.deathManager = deathManager;
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }

        public void Initialize()
        {
            isActive = false;
        }

        public void Tick()
        {
            if (!isActive)
            {
                return;
            }

            List<PlayerFacade> alivePlayerFacades = deathManager.AlivePlayerFacades;
            alivePlayerFacades.Sort((a, b) => b.GroundingPosition.y.CompareTo(a.GroundingPosition.y));

            for (int rank = 0; rank < alivePlayerFacades.Count; ++rank)
            {
                PlayerFacade playerFacade = alivePlayerFacades[rank];
                playerFacade.Player.CurrentRank = rank;
                Input input = playerManager.Input(playerFacade.PlayerView);
                displayManager.UpdateRank(input.DeviceNum, rank);
            }
        }
    }
}
