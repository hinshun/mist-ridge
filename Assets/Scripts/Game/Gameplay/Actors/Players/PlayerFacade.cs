using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly PlayerView playerView;

        public PlayerFacade(
            PlayerView playerView,
            PlayerStateMachine playerStateMachine)
        {
            this.playerView = playerView;
        }

        public PlayerView PlayerView
        {
            get
            {
                return playerView;
            }
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
