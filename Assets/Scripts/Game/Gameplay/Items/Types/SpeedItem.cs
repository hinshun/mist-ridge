using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class SpeedItem : ConsumableItem<SpeedItemEffect>
    {
        private bool active;

        public SpeedItem(
                Player player,
                SpeedItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            active = false;
        }

        public override void Tick()
        {
            if (active)
            {
                Debug.Log("Speed: " + player.WalkSpeed);
                player.WalkSpeed = Mathf.Lerp(1f, 5f, Time.deltaTime);
            }
        }
    }
}
