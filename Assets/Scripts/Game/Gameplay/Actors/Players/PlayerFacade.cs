using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade, ITickable
    {
        private readonly PlayerView playerView;
        private readonly PlayerController playerController;

        public PlayerFacade(
            PlayerView playerView,
            PlayerStateMachine playerStateMachine,
            PlayerController playerController)
        {
            this.playerView = playerView;
            this.playerController = playerController;
        }

        public PlayerView PlayerView
        {
            get
            {
                return playerView;
            }
        }

        public void Tick()
        {
            playerController.Tick();
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
