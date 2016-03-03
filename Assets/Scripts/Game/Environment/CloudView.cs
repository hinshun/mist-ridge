using UnityEngine;
using System;

namespace MistRidge
{
    public class CloudView : PoolInstanceView
    {
        private EllipsoidParticleEmitter cloudEmitter;

        public override void OnPoolInstanceReuse()
        {
            cloudEmitter.emit = false;
            cloudEmitter.emit = true;
        }

        private void Awake()
        {
            cloudEmitter = GetComponent<EllipsoidParticleEmitter>();
        }
    }
}
