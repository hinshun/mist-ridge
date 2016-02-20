using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class MenuSignal : Signal<MenuSignalType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
