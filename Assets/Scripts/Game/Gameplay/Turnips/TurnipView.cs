using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class TurnipView : MonoView
    {
        [SerializeField]
        private Transform emoteTransform;

        [SerializeField]
        private EmoteView emoteView;

        private PoolManager poolManager;
        private float timer;
        private bool emoted;

        [PostInject]
        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        public void Update()
        {
            if (!emoted && Time.time - timer >  2f)
            {
                emoted = true;
                poolManager.ReusePoolInstance(
                    emoteView,
                    emoteTransform.position,
                    Quaternion.identity
                );
            }
        }

        private void Awake()
        {
            timer = Time.time;
            emoted = false;
        }
    }
}
