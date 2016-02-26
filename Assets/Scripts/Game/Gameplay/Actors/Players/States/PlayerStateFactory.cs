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
            switch (stateType)
            {
                case PlayerStateType.Idle:
                    return container.Instantiate<PlayerIdleState>();

                case PlayerStateType.Walk:
                    return container.Instantiate<PlayerWalkState>();

                case PlayerStateType.Jump:
                    return container.Instantiate<PlayerJumpState>();

                case PlayerStateType.Fall:
                    return container.Instantiate<PlayerFallState>();
            }

            Debug.LogError("Failed to create valid player state");
            return null;
        }
    }
}
