using UnityEngine;
using System.Collections;
using Zenject;
using ModestTree;

namespace MistRidge
{
    public class PlayerStateFactory : StateFactory<PlayerStateMachine, PlayerBaseState, PlayerStateType, PlayerStateFactory>
    {
        public PlayerStateFactory(DiContainer container)
            : base(container) {}

        public override PlayerBaseState Create(PlayerStateType stateType, params object[] constructorArgs)
        {
            switch (stateType)
            {
                case PlayerStateType.Idle:
                    return container.Instantiate<PlayerIdleState>(constructorArgs);

                case PlayerStateType.Walk:
                    return container.Instantiate<PlayerWalkState>(constructorArgs);

                case PlayerStateType.Jump:
                    return container.Instantiate<PlayerJumpState>(constructorArgs);

                case PlayerStateType.Fall:
                    return container.Instantiate<PlayerFallState>(constructorArgs);
            }

            Assert.That(false);
            return null;
        }
    }
}
