using UnityEngine;
using System;

namespace MistRidge
{
    public class CloudView : PoolInstanceView
    {
        [SerializeField]
        private float moveSpeed;

        new private ParticleSystem particleSystem;

        public override void OnPoolInstanceReuse()
        {
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.enabled = false;
            emission.enabled = true;
        }

        private void Update()
        {
            Position += moveSpeed * Up * Time.deltaTime;
        }

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
    }
}
