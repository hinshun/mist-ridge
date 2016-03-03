using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class MenuSignal : Signal<Input, MenuSignalType>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
