using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class QuicknessItem : ConsumableItem<QuicknessItemEffect>
    {
        private float afterImageTimer;
        private float timer;

        public QuicknessItem(
                Player player,
                QuicknessItemEffect itemEffect)
            : base(player, itemEffect)
        {
        }

        public override void Tick()
        {
            if (Time.time - timer > itemEffect.Duration && !isDisposable && !IsUsable())
            {
                isDisposable = true;
                return;
            }

            if (Time.time - afterImageTimer > itemEffect.AfterImageDelay)
            {
                player.AfterImage();
                afterImageTimer = Time.time;
            }
        }

        public override void Dispose()
        {
            player.WalkSpeed = 1f;
            player.JumpSpeed = 1f;
        }

        public override void OnUse()
        {
            timer = Time.time;
            afterImageTimer = Time.time;
            player.WalkSpeed = itemEffect.WalkSpeed;
            player.JumpSpeed = itemEffect.JumpSpeed;
        }
    }
}
