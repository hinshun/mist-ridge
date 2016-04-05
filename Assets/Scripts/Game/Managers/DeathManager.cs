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
        private readonly SpawnManager spawnManager;
        private readonly DisplayManager displayManager;
        private readonly RespawnSignal respawnSignal;
        private readonly CheckpointActionSignal.Trigger checkpointActionTrigger;

        private bool isTutorial;
        private bool isActive;
        private Dictionary<PlayerFacade, float> deathTimers;
        private Dictionary<PlayerFacade, bool> playerDeaths;

        public DeathManager(
                Settings settings,
                Camera camera,
                InputManager inputManager,
                PlayerManager playerManager,
                SpawnManager spawnManager,
                DisplayManager displayManager,
                RespawnSignal respawnSignal,
                CheckpointActionSignal.Trigger checkpointActionTrigger)
        {
            this.settings = settings;
            this.camera = camera;
            this.inputManager = inputManager;
            this.playerManager = playerManager;
            this.spawnManager = spawnManager;
            this.displayManager = displayManager;
            this.respawnSignal = respawnSignal;
            this.checkpointActionTrigger = checkpointActionTrigger;
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

        public bool IsTutorial
        {
            get
            {
                return isTutorial;
            }
            set
            {
                isTutorial = value;
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

        public List<PlayerFacade> DeadPlayerFacades
        {
            get
            {
                List<PlayerFacade> deadPlayerFacades = new List<PlayerFacade>();

                foreach (KeyValuePair<PlayerFacade, bool> entry in playerDeaths)
                {
                    if (entry.Value)
                    {
                        deadPlayerFacades.Add(entry.Key);
                    }
                }

                return deadPlayerFacades;
            }
        }

        public int DeadPlayerCount
        {
            get
            {
                return DeadPlayerFacades.Count;
            }
        }

        public int AlivePlayerCount
        {
            get
            {
                return AlivePlayerFacades.Count;
            }
        }

        public List<Vector3> AlivePlayerPositions
        {
            get
            {
                List<Vector3> playerPositions = new List<Vector3>();
                foreach (PlayerFacade playerFacade in AlivePlayerFacades)
                {
                    playerPositions.Add(playerFacade.Position);
                }

                return playerPositions;
            }
        }

        public List<Vector3> AliveRelevantPlayerPositions
        {
            get
            {
                return RelevantPositions(AlivePlayerPositions);
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
            isTutorial = true;
            respawnSignal.Event += OnPlayerRespawn;
            deathTimers = new Dictionary<PlayerFacade, float>();
            playerDeaths = new Dictionary<PlayerFacade, bool>();
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
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);

                if (playerFacade == null || !playerDeaths.ContainsKey(playerFacade) || playerDeaths[playerFacade])
                {
                    continue;
                }

                if (GeometryUtility.TestPlanesAABB(planes, playerFacade.Bounds))
                {
                    DeathTimerReset(input, playerFacade);
                }
                else
                {
                    DeathTimerTick(input, playerFacade);
                }
            }
        }

        public void AddPlayer(Input input)
        {
            if (!playerManager.HasPlayerFacade(input))
            {
                return;
            }

            PlayerFacade playerFacade = playerManager.PlayerFacade(input);

            if (playerFacade == null)
            {
                return;
            }

            if (!deathTimers.ContainsKey(playerFacade))
            {
                deathTimers.Add(playerFacade, 0f);
            }

            if (!playerDeaths.ContainsKey(playerFacade))
            {
                playerDeaths.Add(playerFacade, false);
            }
        }

        public void DeathTimerTick(Input input, PlayerFacade playerFacade)
        {
            Vector3 position = camera.WorldToViewportPoint(playerFacade.Position);
            displayManager.UpdatePointer(input.DeviceNum, position);

            if (deathTimers[playerFacade] == 0)
            {
                deathTimers[playerFacade] = Time.time;
            }

            if (Time.time - deathTimers[playerFacade] > settings.deathTimeLimit)
            {
                Kill(playerFacade);
                DeathTimerReset(input, playerFacade);
            }
        }

        public void DeathTimerReset(Input input, PlayerFacade playerFacade)
        {
            deathTimers[playerFacade] = 0;
            displayManager.UpdatePointer(input.DeviceNum, Vector2.zero);
        }

        public void Kill(PlayerFacade playerFacade)
        {
            Input input = playerManager.Input(playerFacade.PlayerView);

            playerDeaths[playerFacade] = true;
            playerFacade.Die();
            displayManager.UpdateBackdrop(input.DeviceNum, BackdropHealth.Dead);
            displayManager.UpdatePortraitImage(input.DeviceNum, playerFacade.CharacterType, PortraitEmotion.Dead);
            displayManager.UpdatePointer(input.DeviceNum, Vector2.zero);
            displayManager.UpdateRank(input.DeviceNum, -1);

            if (isTutorial || AlivePlayerCount == 0)
            {
                checkpointActionTrigger.Fire(CheckpointAction.Respawn);
            }
            else
            {
                checkpointActionTrigger.Fire(CheckpointAction.Finish);
            }
        }

        public void Respawn(PlayerFacade playerFacade)
        {
            if (!playerDeaths[playerFacade])
            {
                return;
            }

            PlayerView playerView = playerFacade.PlayerView;
            Transform spawnTransform = spawnManager.CurrentSpawnView.SpawnTransform(playerFacade.Input.DeviceNum);

            ParticleTargetRequest particleTargetRequest = new ParticleTargetRequest()
            {
                particleSystem = playerView.Respawn,
                particleCount = 1,
                targetTime = Mathf.Sqrt((playerView.Position - spawnTransform.position).magnitude) * settings.respawnTargetFactor,
                targetTransform = spawnTransform,
                particleTargetType = ParticleTargetType.Respawn,
                playerFacade = playerFacade,
            };

            spawnManager.CurrentSpawnView.ParticleTargetView.Target(particleTargetRequest);
        }

        private void OnPlayerRespawn(PlayerFacade playerFacade)
        {
            if (!playerDeaths[playerFacade])
            {
                return;
            }

            Input input = playerManager.Input(playerFacade.PlayerView);

            playerDeaths[playerFacade] = false;
            displayManager.UpdateBackdrop(input.DeviceNum, BackdropHealth.Alive);
            displayManager.UpdatePortraitImage(input.DeviceNum, playerFacade.CharacterType, PortraitEmotion.Neutral);
            playerFacade.Position = spawnManager.CurrentSpawnView.SpawnPoint(playerFacade.Input.DeviceNum);
            playerFacade.MoveDirection = Vector3.zero;

            playerFacade.Respawn();
        }

        private List<Vector3> RelevantPositions(List<Vector3> positions)
        {
            if (positions.Count == 0)
            {
                return positions;
            }

            positions.Sort((a, b) => a.y.CompareTo(b.y));

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
            public float deathTimeLimit;
            public float respawnTargetFactor;
        }
    }
}
