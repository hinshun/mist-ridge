using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly Grounding grounding;
        private readonly PlayerView playerView;
        private readonly PlayerStateMachine playerStateMachine;

        public PlayerFacade(
            Grounding grounding,
            PlayerView playerView,
            PlayerStateMachine playerStateMachine)
        {
            this.grounding = grounding;
            this.playerView = playerView;
            this.playerStateMachine = playerStateMachine;
        }

        public Vector3 Position
        {
            get
            {
                return playerView.Position;
            }
            set
            {
                playerView.Position = value;
            }
        }

        public Vector3 GroundingPosition
        {
            get
            {
                return grounding.PrimaryPoint;
            }
        }

        public void Freefall()
        {
            playerStateMachine.ChangeState(PlayerStateType.Freefall);
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
