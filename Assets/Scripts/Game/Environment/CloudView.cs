using UnityEngine;
using System;

namespace MistRidge
{
    public class CloudView : PoolInstanceView
    {
        [SerializeField]
        private float moveSpeed;

        private EllipsoidParticleEmitter cloudEmitter;

        public override void OnPoolInstanceReuse()
        {
            cloudEmitter.emit = false;
            cloudEmitter.emit = true;
        }

        private void Update()
        {
            Position += moveSpeed * Up * Time.deltaTime;
        }

        private void Awake()
        {
            cloudEmitter = GetComponent<EllipsoidParticleEmitter>();
        }
    }
}
