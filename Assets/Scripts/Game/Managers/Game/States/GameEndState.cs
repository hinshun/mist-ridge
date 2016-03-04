using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class GameEndState : GameBaseState
    {
        private readonly AetherManager aetherManager;
        private readonly PlayerManager playerManager;

        public GameEndState(
                AetherManager aetherManager,
                PlayerManager playerManager,
                GameStateMachine stateMachine)
            : base(stateMachine)
        {
            this.aetherManager = aetherManager;
            this.playerManager = playerManager;

            stateType = GameStateType.End;
        }

        public override void Update()
        {
            // Do Nothing
        }

        public override void EnterState()
        {
            PlayerView leadPlayerView = aetherManager.LeadPlayerView;
            Input leadInput = playerManager.Input(leadPlayerView);
            int aethers = aetherManager.Aethers(leadPlayerView);

            Debug.Log("Player " + leadInput.DeviceNum + " has won the game with " + aethers + " aethers");
        }

        public override void ExitState()
        {
            // Do Nothing
        }
    }
}
