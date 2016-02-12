using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly PlayerView view;

        public PlayerFacade(PlayerView view)
        {
            this.view = view;
        }

        public PlayerView View
        {
            get
            {
                return view;
            }
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
