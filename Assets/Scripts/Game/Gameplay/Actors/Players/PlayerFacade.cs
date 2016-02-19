using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade, IInitializable, ITickable
    {
        private readonly Input input;
        private readonly PlayerView playerView;

        public PlayerFacade(
            Input input,
            PlayerView playerView,
            PlayerStateMachine playerStateMachine)
        {
            this.input = input;
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
