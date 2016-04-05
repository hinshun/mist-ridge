using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class AetherManager : IInitializable
    {
        private readonly Settings settings;
        private readonly PlayerManager playerManager;
        private readonly DisplayManager displayManager;
        private readonly AetherGainSignal aetherGainSignal;

        private Dictionary<PlayerView, int> playerAethers;
        private float gameTimer;

        public AetherManager(
                Settings settings,
                PlayerManager playerManager,
                DisplayManager displayManager,
                AetherGainSignal aetherGainSignal)
        {
            this.settings = settings;
            this.playerManager = playerManager;
            this.displayManager = displayManager;
            this.aetherGainSignal = aetherGainSignal;
        }

        public float GameTimer
        {
            get
            {
                return gameTimer;
            }
            set
            {
                gameTimer = value;
            }
        }

        public void Initialize()
        {
            aetherGainSignal.Event += OnAetherGain;
            playerAethers = new Dictionary<PlayerView, int>();
        }

        public Dictionary<PlayerView, ScorePlacementType> Placements
        {
            get
            {
                List<PlayerView> playerViews = new List<PlayerView>();
                playerViews.AddRange(playerAethers.Keys);

                playerViews.Sort((a, b) => playerAethers[b].CompareTo(playerAethers[a]));

                Dictionary<PlayerView, ScorePlacementType> placements = new Dictionary<PlayerView, ScorePlacementType>();

                ScorePlacementType[] orderedPlacements = new ScorePlacementType[] {
                    ScorePlacementType.First,
                    ScorePlacementType.Second,
                    ScorePlacementType.Third,
                    ScorePlacementType.Fourth,
                };

                for (int i = 0; i < playerViews.Count; ++i)
                {
                    placements.Add(playerViews[i], orderedPlacements[i]);
                }

                return placements;
            }
        }

        public int Aethers(PlayerView playerView)
        {
            return playerAethers[playerView];
        }

        public void AddAether(CheckpointView checkpointView, PlayerView playerView, int aetherCount)
        {
            ParticleTargetRequest particleTargetRequest = new ParticleTargetRequest()
            {
                particleSystem = checkpointView.AetherAward,
                particleCount = aetherCount,
                targetTime = settings.aetherTargetTime,
                targetTransform = playerView.transform,
                particleTargetType = ParticleTargetType.Aether,
                playerView = playerView,
            };

            checkpointView.ParticleTargetView.Target(particleTargetRequest);
        }

        public void OnAetherGain(PlayerView playerView)
        {
            if (!playerAethers.ContainsKey(playerView))
            {
                playerAethers.Add(playerView, 0);
            }

            playerAethers[playerView]++;
            Input input = playerManager.Input(playerView);
            displayManager.UpdateAether(input.DeviceNum, playerAethers[playerView]);
        }

        [Serializable]
        public class Settings
        {
            public float aetherTargetTime;
        }
    }
}
