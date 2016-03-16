using UnityEngine;
using System;

namespace MistRidge
{
    public class BubbleTrapView : PoolInstanceView
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float height;

        private bool activated;
        private PlayerView heldPlayerView;

        public float Height
        {
            get
            {
                return height;
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }
        }

        public override void OnPoolInstanceReuse()
        {
            activated = false;
            heldPlayerView = null;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (activated)
            {
            }

            PlayerView playerView = other.GetComponent<PlayerView>();
        }
    }
}
