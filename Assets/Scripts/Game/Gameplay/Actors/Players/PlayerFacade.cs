using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly Grounding grounding;
        private readonly Input input;
        private readonly Player player;
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;
        private readonly PlayerStateMachine playerStateMachine;

        public PlayerFacade(
            Grounding grounding,
            Input input,
            Player player,
            PlayerView playerView,
            PlayerController playerController,
            PlayerStateMachine playerStateMachine)
        {
            this.grounding = grounding;
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.playerController = playerController;
            this.playerStateMachine = playerStateMachine;
        }

        public Input Input
        {
            get
            {
                return input;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }
        }

        public PlayerView PlayerView
        {
            get
            {
                return playerView;
            }
        }

        public Vector3 MoveDirection
        {
            get
            {
                return playerStateMachine.MoveDirection;
            }
            set
            {
                playerStateMachine.MoveDirection = value;
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

        public bool IsAlive
        {
            get
            {
                return player.IsAlive;
            }
            set
            {
                player.IsAlive = value;
            }
        }

        public void ProbeGround()
        {
            playerController.ProbeGround();
        }

        public void Freefall()
        {
            playerStateMachine.ChangeState(PlayerStateType.Freefall);
        }

        public void Respawn()
        {
            playerView.PlayerAllowJump();
            IsAlive = true;
        }

        public void Die()
        {
            IsAlive = false;
        }

        public void Dance()
        {
            playerView.Animator.SetTrigger("StartDance");
            playerStateMachine.ChangeState(PlayerStateType.Uncontrollable);
        }

        public void StopDance()
        {
            playerView.CanControl = true;
            playerView.Animator.SetTrigger("StopDance");
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
