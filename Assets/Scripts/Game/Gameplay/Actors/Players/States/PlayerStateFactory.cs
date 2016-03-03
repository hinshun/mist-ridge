using UnityEngine;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class PlayerStateFactory : StateFactory<PlayerStateMachine, PlayerBaseState, PlayerStateType, PlayerStateFactory>
    {
        public PlayerStateFactory(DiContainer container)
            : base(container) {}

        public override PlayerBaseState Create(PlayerStateType stateType)
        {
            PlayerBaseState playerState = null;

            switch (stateType)
            {
                case PlayerStateType.Idle:
                    playerState = container.Instantiate<PlayerIdleState>();
                    break;

                case PlayerStateType.Walk:
                    playerState = container.Instantiate<PlayerWalkState>();
                    break;

                case PlayerStateType.Jump:
                    playerState = container.Instantiate<PlayerJumpState>();
                    break;

                case PlayerStateType.Fall:
                    playerState = container.Instantiate<PlayerFallState>();
                    break;

                case PlayerStateType.Freefall:
                    playerState = container.Instantiate<PlayerFreefallState>();
                    break;
            }

            if (playerState == null)
            {
                Debug.LogError("Failed to create valid player state");
                return null;
            }

            playerState.Initialize();
            return playerState;
        }
    }
}
