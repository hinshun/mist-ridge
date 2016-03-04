using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class CheckpointSignal : Signal<CheckpointView, PlayerView>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
