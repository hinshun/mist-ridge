using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
