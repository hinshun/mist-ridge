using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CinematicManager : IInitializable
    {
        private readonly DeathManager deathManager;

        private CinematicType cinematicType;
        private StartingZoneView startingZoneView;

        public CinematicManager(DeathManager deathManager)
        {
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

        public void Initialize()
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
                        return deathManager.AliveRelevantPlayerPositions;

                    case CinematicType.StartingZone:
                        List<Vector3> turnipPositions = new List<Vector3>();
                        turnipPositions.Add(startingZoneView.TurnipView.Position);

                        return turnipPositions;

                    case CinematicType.PeakZone:
                        return new List<Vector3>();
                }

                return new List<Vector3>();
            }
        }
    }
}
