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

        public CinematicManager(
                SpawnManager spawnManager,
                DeathManager deathManager)
        {
            this.spawnManager = spawnManager;
            this.deathManager = deathManager;
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

                    case CinematicType.StartingZone:
                        List<Vector3> turnipPositions = new List<Vector3>();
                        turnipPositions.Add(startingZoneView.TurnipView.Position);

                        return turnipPositions;

                    case CinematicType.PeakZone:
                        List<Vector3> peakPositions = new List<Vector3>();
                        peakPositions.Add(peakZoneView.PeakTransform.position);

                        return peakPositions;
                }

                return new List<Vector3>();
            }
        }
    }
}
