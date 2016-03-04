using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class DeathManager : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly Camera camera;
        private readonly InputManager inputManager;
        private readonly PlayerManager playerManager;

        private bool isActive;
        private Dictionary<PlayerFacade, bool> playerDeaths;

        public DeathManager(
                Settings settings,
                Camera camera,
                InputManager inputManager,
                PlayerManager playerManager)
        {
            this.settings = settings;
            this.camera = camera;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
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

        public List<PlayerFacade> AlivePlayerFacades
        {
            get
            {
                List<PlayerFacade> alivePlayerFacades = new List<PlayerFacade>();

                foreach (KeyValuePair<PlayerFacade, bool> entry in playerDeaths)
                {
                    if (!entry.Value)
                    {
                        alivePlayerFacades.Add(entry.Key);
                    }
                }

                return alivePlayerFacades;
            }
        }

        public int AlivePlayerCount
        {
            get
            {
                return AlivePlayerFacades.Count;
            }
        }

        public List<Vector3> AlivePlayerGroundingPositions
        {
            get
            {
                List<Vector3> playerPositions = new List<Vector3>();
                foreach (PlayerFacade playerFacade in AlivePlayerFacades)
                {
                    playerPositions.Add(playerFacade.GroundingPosition);
                }

                return playerPositions;
            }
        }

        public List<Vector3> AliveRelevantPlayerGroundingPositions
        {
            get
            {
                return RelevantPositions(AlivePlayerGroundingPositions);
            }
        }

        public void Initialize()
        {
            isActive = false;
            playerDeaths = new Dictionary<PlayerFacade, bool>();
        }

        public void PopulatePlayerDeaths()
        {
            foreach (Input input in inputManager.Inputs)
            {
                PlayerFacade playerFacade = playerManager.PlayerFacade(input);

                if (playerFacade == null)
                {
                    continue;
                }

                if (!playerDeaths.ContainsKey(playerFacade))
                {
                    playerDeaths.Add(playerFacade, false);
                }
            }
        }

        public void Tick()
        {
            if (!isActive)
            {
                return;
            }

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            foreach (Input input in inputManager.Inputs)
            {
                PlayerFacade playerFacade = playerManager.PlayerFacade(input);

                if (playerFacade == null || !playerDeaths.ContainsKey(playerFacade) || playerDeaths[playerFacade])
                {
                    continue;
                }

                if (!GeometryUtility.TestPlanesAABB(planes, playerFacade.Bounds))
                {
                    playerDeaths[playerFacade] = true;
                    playerFacade.Die();
                }
            }
        }

        private void SortRankedPositions(List<Vector3> positions)
        {
            positions.Sort((a, b) => a.y.CompareTo(b.y));
        }

        private List<Vector3> RelevantPositions(List<Vector3> positions)
        {
            SortRankedPositions(positions);

            Vector3 firstPlayerPosition = positions[positions.Count - 1];
            Vector3 lastPlayerPosition = positions[0];

            float playerDistance = (firstPlayerPosition - lastPlayerPosition).magnitude;
            float relevantDistance = Mathf.Max(settings.minRelevantDistance, settings.relevance * playerDistance);

            List<Vector3> relevantPositions = new List<Vector3>();
            foreach (Vector3 position in positions)
            {
                Vector3 firstPlayerDirection = firstPlayerPosition - position;
                float magnitude = firstPlayerDirection.magnitude + (firstPlayerDirection.y * settings.altitudeWeight);

                if (magnitude <= relevantDistance)
                {
                    relevantPositions.Add(position);
                }
                else
                {
                    float displacement = magnitude - relevantDistance;
                    relevantPositions.Add(Vector3.MoveTowards(position, firstPlayerPosition, settings.displacementWeight * displacement));
                }
            }

            return relevantPositions;
        }

        [Serializable]
        public class Settings
        {
            public float relevance;
            public float minRelevantDistance;
            public float displacementWeight;
            public float altitudeWeight;
        }
    }
}
