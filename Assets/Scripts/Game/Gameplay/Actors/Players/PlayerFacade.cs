using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly Grounding grounding;
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;
        private readonly PlayerStateMachine playerStateMachine;

        public PlayerFacade(
            Grounding grounding,
            PlayerView playerView,
            PlayerController playerController,
            PlayerStateMachine playerStateMachine)
        {
            this.grounding = grounding;
            this.playerView = playerView;
            this.playerController = playerController;
            this.playerStateMachine = playerStateMachine;
        }

        public PlayerView PlayerView
        {
            get
            {
                return playerView;
            }
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
                playerController.ProbeGround();
            }
        }

        public Transform Parent
        {
            get
            {
                return playerView.Parent;
            }
            set
            {
                playerView.Parent = value;
            }
        }

        public Vector3 GroundingPosition
        {
            get
            {
                return grounding.PrimaryPoint;
            }
        }

        public Bounds Bounds
        {
            get
            {
                return playerView.Collider.bounds;
            }
        }

        public void Freefall()
        {
            playerStateMachine.ChangeState(PlayerStateType.Freefall);
        }

        public void Die()
        {
            Debug.Log("I'm dead");
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
