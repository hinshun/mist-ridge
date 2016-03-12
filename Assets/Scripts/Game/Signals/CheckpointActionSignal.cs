using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class CheckpointActionSignal : Signal<CheckpointAction>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
