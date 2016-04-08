using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class GameEndState : GameBaseState
    {
        private readonly Settings settings;
        private readonly Generator generator;
        private readonly SceneLoadSignal sceneLoadSignal;
        private readonly SceneLoader sceneLoader;
        private readonly InputManager inputManager;
        private readonly AetherManager aetherManager;
        private readonly PlayerManager playerManager;
        private readonly DialogueManager dialogueManager;
        private readonly DeathManager deathManager;
        private readonly CameraManager cameraManager;
        private readonly CameraAnchorManager cameraAnchorManager;
        private readonly CameraRigManager cameraRigManager;
        private readonly CinematicManager cinematicManager;
        private readonly DisplayManager displayManager;
        private readonly MistManager mistManager;
        private readonly RankManager rankManager;
        private readonly SpawnManager spawnManager;
        private readonly PoolManager poolManager;
        private readonly SprintManager sprintManager;

        private bool menuShown;

        private Vector3[] path;
        private Hashtable spiralHashtable;
        private Hashtable bloomHashtable;
        private Hashtable bloomSteadyHashtable;

        public GameEndState(
                Settings settings,
                Generator generator,
                SceneLoadSignal sceneLoadSignal,
                SceneLoader sceneLoader,
                InputManager inputManager,
                AetherManager aetherManager,
                PlayerManager playerManager,
                DialogueManager dialogueManager,
                DeathManager deathManager,
                CameraManager cameraManager,
                CameraAnchorManager cameraAnchorManager,
                CameraRigManager cameraRigManager,
                CinematicManager cinematicManager,
                DisplayManager displayManager,
                MistManager mistManager,
                RankManager rankManager,
                SpawnManager spawnManager,
                PoolManager poolManager,
                SprintManager sprintManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.settings = settings;
            this.generator = generator;
            this.sceneLoadSignal = sceneLoadSignal;
            this.sceneLoader = sceneLoader;
            this.inputManager = inputManager;
            this.aetherManager = aetherManager;
            this.playerManager = playerManager;
            this.dialogueManager = dialogueManager;
            this.deathManager = deathManager;
            this.cameraManager = cameraManager;
            this.cameraAnchorManager = cameraAnchorManager;
            this.cameraRigManager = cameraRigManager;
            this.cinematicManager = cinematicManager;
            this.displayManager = displayManager;
            this.mistManager = mistManager;
            this.rankManager = rankManager;
            this.spawnManager = spawnManager;
            this.poolManager = poolManager;
            this.sprintManager = sprintManager;

            stateType = GameStateType.End;
        }

        public override void Initialize()
        {
            sceneLoadSignal.Event += OnSceneLoad;

            spiralHashtable = new Hashtable();
            spiralHashtable.Add("easetype", iTween.EaseType.easeInOutSine);

            bloomHashtable = new Hashtable();
            bloomHashtable.Add("from", 0);
            bloomHashtable.Add("to", 1);
            bloomHashtable.Add("time", settings.bloomTime);
            bloomHashtable.Add("onupdate", "OnBloom");
            bloomHashtable.Add("oncomplete", "OnBloomComplete");

            bloomSteadyHashtable = new Hashtable();
            bloomSteadyHashtable.Add("from", 0);
            bloomSteadyHashtable.Add("to", 1);
            bloomSteadyHashtable.Add("time", settings.bloomSteadyTime);
            bloomSteadyHashtable.Add("onupdate", "OnBloomSteady");

            ResetVariables();
        }

        public void ResetVariables()
        {
            menuShown = false;
        }

        public override void Update()
        {
            if (!menuShown)
            {
                return;
            }

            foreach(Input input in inputManager.Inputs)
            {
                if (input.Mapping.MenuWasPressed)
                {
                    ResetGame();
                }
            }
        }

        public override void EnterState()
        {
            deathManager.IsActive = false;
            sceneLoader.OnlyFade();
            ComputeSpiralPath();
        }

        public override void ExitState()
        {
            cinematicManager.CinematicType = CinematicType.None;
        }

        public void OnBloomComplete()
        {
            PeakView peakView = stateMachine.ChunkManager.PeakZoneView.PeakView;
            iTween.ValueTo(peakView.gameObject, bloomSteadyHashtable);
        }

        private void ComputeSpiralPath()
        {
            ChunkManager chunkManager = stateMachine.ChunkManager;

            path = new Vector3[chunkManager.ChunkCount - 1];

            int i = 0;
            ChunkFacade previousChunkFacade = chunkManager.PeakChunkFacade.PreviousChunkFacade;

            while (previousChunkFacade != null)
            {
                if (previousChunkFacade.PreviousChunkFacade == null)
                {
                    StartingZoneView startingZoneView = previousChunkFacade.ChunkView.GetComponentInChildren<StartingZoneView>();
                    path[i] = startingZoneView.NormalSpawn.Position;
                }
                else
                {
                    path[i] = previousChunkFacade.ChunkView.Position + (Vector3.up * previousChunkFacade.ChunkView.ChunkFeatureView.EntryAltitude);
                }

                i++;
                previousChunkFacade = previousChunkFacade.PreviousChunkFacade;
            }
        }

        private void OnSceneLoad()
        {
            displayManager.UpdateSprint(false);
            DisablePlayerDisplays();
            cameraManager.ResetCamera();
            cameraManager.ZoomOverride = settings.zoomOverride;
            cameraManager.ZoomOverrideEnabled = true;
            cameraAnchorManager.ResetAnchor();
            cameraRigManager.RigPosition = settings.rigPosition;
            cinematicManager.CinematicType = CinematicType.PeakZone;

            Dictionary<PlayerView, ScorePlacementType> placements = aetherManager.Placements;

            foreach (Input input in inputManager.Inputs)
            {
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                PlayerView playerView = playerFacade.PlayerView;
                ScorePlacementType scorePlacementType = placements[playerView];

                displayManager.UpdateScorePlayer(
                    input.DeviceNum,
                    aetherManager.Aethers(playerView),
                    scorePlacementType,
                    playerFacade.CharacterType
                );
            }

            displayManager.UpdateScoreTime(true, Mathf.FloorToInt(Time.time - aetherManager.GameTimer));
            displayManager.UpdateScoreMenu(true);

            menuShown = true;

            SpiralCamera();
        }

        private void SpiralCamera()
        {
            spiralHashtable["path"] = path;
            spiralHashtable["time"] = path.Length * settings.spiralTimePerChunk;

            PeakView peakView = stateMachine.ChunkManager.PeakZoneView.PeakView;
            peakView.GameEndState = this;

            iTween.MoveTo(peakView.gameObject, spiralHashtable);
            iTween.ValueTo(peakView.gameObject, bloomHashtable);
        }

        private void DisablePlayerDisplays()
        {
            foreach (Input input in inputManager.Inputs)
            {
                if (!playerManager.HasPlayerFacade(input))
                {
                    continue;
                }

                PlayerFacade playerFacade = playerManager.PlayerFacade(input);
                displayManager.Display(input.DeviceNum, CharacterType.None);
            }
        }

        private void ResetGame()
        {
            ResetVariables();

            cameraManager.ResetVariables();
            cameraRigManager.ResetVariables();
            dialogueManager.ResetVariables();
            stateMachine.GameReadyState.ResetVariables();
            aetherManager.ResetVariables();
            cinematicManager.ResetVariables();
            deathManager.ResetVariables();
            displayManager.ResetVariables();
            mistManager.ResetVariables();
            playerManager.ResetVariables();
            rankManager.ResetVariables();
            poolManager.ResetVariables();
            sprintManager.ResetVariables();
            spawnManager.CurrentSpawnView = null;
            generator.ResetVariables();

            displayManager.UpdateCinematic(false);
            stateMachine.ChangeState(GameStateType.Start);
            sceneLoader.Load(settings.startMenuSceneName);
        }

        [Serializable]
        public class Settings
        {
            public string startMenuSceneName;

            public float spiralTimePerChunk;

            public float bloomTime;
            public float bloomSteadyTime;

            public float zoomOverride;
            public Vector3 rigPosition;
        }
    }
}
