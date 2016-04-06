using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CinematicManager : IInitializable
    {
        private readonly SpawnManager spawnManager;
        private readonly DeathManager deathManager;

        private CinematicType cinematicType;
        private StartingZoneView startingZoneView;
        private PeakZoneView peakZoneView;
        private PlayerView triggerPlayerView;

        public CinematicManager(
                SpawnManager spawnManager,
                DeathManager deathManager)
        {
            this.spawnManager = spawnManager;
            this.deathManager = deathManager;
        }

        public PlayerView TriggerPlayerView
        {
            get
            {
                return triggerPlayerView;
            }
            set
            {
                triggerPlayerView = value;
            }
        }

        public CinematicType CinematicType
        {
            get
            {
                return cinematicType;
            }
            set
            {
                cinematicType = value;
            }
        }

        public StartingZoneView StartingZoneView
        {
            get
            {
                return startingZoneView;
            }
            set
            {
                startingZoneView = value;
            }
        }

        public PeakZoneView PeakZoneView
        {
            get
            {
                return peakZoneView;
            }
            set
            {
                peakZoneView = value;
            }
        }

        public void Initialize()
        {
            ResetVariables();
        }

        public void ResetVariables()
        {
            cinematicType = CinematicType.None;
        }

        public List<Vector3> Positions
        {
            get
            {
                switch(cinematicType)
                {
                    case CinematicType.None:
                        List<Vector3> positions = deathManager.AliveRelevantPlayerPositions;

                        if (positions.Count == 0)
                        {
                            positions.Add(spawnManager.CurrentSpawnView.Position);
                        }

                        return positions;

                    case CinematicType.Turnip:
                        return GetTurnipPositions();

                    case CinematicType.TurnipAndPlayer:
                        List<Vector3> turnipPlayerPositions = GetTurnipPositions();
                        turnipPlayerPositions.Add(triggerPlayerView.Position);

                        return turnipPlayerPositions;

                    case CinematicType.TurnipAndLantern:
                        List<Vector3> turnipLanternPositions = GetTurnipPositions();
                        turnipLanternPositions.Add(startingZoneView.NormalSpawn.Position);

                        return turnipLanternPositions;

                    case CinematicType.PeakZone:
                        List<Vector3> peakPositions = new List<Vector3>();
                        peakPositions.Add(peakZoneView.PeakTransform.position);

                        return peakPositions;
                }

                return new List<Vector3>();
            }
        }

        private List<Vector3> GetTurnipPositions()
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(startingZoneView.TurnipView.Position);

            return positions;
        }
    }
}
