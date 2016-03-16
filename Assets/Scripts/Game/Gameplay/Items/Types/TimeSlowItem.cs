using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class TimeSlowItem : ConsumableItem<TimeSlowItemEffect>
    {
        public TimeSlowItem(
                Player player,
                TimeSlowItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }
    }
}
