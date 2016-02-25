using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly Grounding grounding;
        private readonly PlayerView playerView;

        public PlayerFacade(
            Grounding grounding,
            PlayerView playerView,
            PlayerStateMachine playerStateMachine)
        {
            this.playerView = playerView;
            this.grounding = grounding;
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

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
