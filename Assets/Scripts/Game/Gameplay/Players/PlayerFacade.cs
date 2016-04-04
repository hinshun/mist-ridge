using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly CharacterType characterType;
        private readonly Grounding grounding;
        private readonly Input input;
        private readonly Player player;
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;
        private readonly PlayerStateMachine playerStateMachine;

        public PlayerFacade(
            CharacterType characterType,
            Grounding grounding,
            Input input,
            Player player,
            PlayerView playerView,
            PlayerController playerController,
            PlayerStateMachine playerStateMachine)
        {
            this.characterType = characterType;
            this.grounding = grounding;
            this.input = input;
            this.player = player;
            this.playerView = playerView;
            this.playerController = playerController;
            this.playerStateMachine = playerStateMachine;
        }

        public CharacterType CharacterType
        {
            get
            {
                return characterType;
            }
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

        public bool Control
        {
            get
            {
                return playerStateMachine.Enabled;
            }
            set
            {
                playerStateMachine.Enabled = value;
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
            Control = false;
        }

        public void StopDance()
        {
            Control = true;
            playerView.Animator.SetTrigger("StopDance");
        }

        public void SetPlayerCircle(Sprite playerCircle)
        {
            playerView.PlayerCircle.sprite = playerCircle;
        }
    }
}
