using Zenject;
using Zenject.Commands;

namespace MistRidge
{
    public class CheckpointSignal : Signal<PlayerView, CheckpointView>
    {
        public class Trigger : TriggerBase
        {
        }
    }
}
