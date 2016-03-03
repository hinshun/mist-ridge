using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpeedItem : ConsumableItem<SpeedItemEffect>
    {
        private float startTime;
        private Vector3 moveDirection;

        public SpeedItem(
                Player player,
                SpeedItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }

        public override void Tick()
        {
            startTime += Time.deltaTime;
            player.MoveDirection += moveDirection * itemEffect.SpeedBoost * Time.deltaTime;

            if (startTime > itemEffect.Duration)
            {
                isActive = false;
                isDisposable = true;
            }
        }

        public override void OnUse()
        {
            startTime = 0;
            moveDirection = player.LookDirection;
        }
    }
}
