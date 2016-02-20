using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class GameStateSignal : Signal<GameStateType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
