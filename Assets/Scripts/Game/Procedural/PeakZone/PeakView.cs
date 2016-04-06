using UnityEngine;
using System;
using System.Collections.Generic;

namespace MistRidge
{
    public class PeakView : MonoView
    {
        [SerializeField]
        private float bloomSpeed;

        [SerializeField]
        private float bloomRadius;

        [SerializeField]
        private float bloomSteadyRadius;

        private float initialRadius;
        private Vector3 fullScale;
        private SphereCollider sphereCollider;
        private GameEndState gameEndState;

        public GameEndState GameEndState
        {
            get
            {
                return gameEndState;
            }
            set
            {
                gameEndState = value;
            }
        }

        public void OnBloom(float time)
        {
            sphereCollider.radius = initialRadius + (time * (bloomRadius - initialRadius));
        }

        public void OnBloomSteady(float time)
        {
            sphereCollider.radius = bloomRadius - (time * (bloomRadius - bloomSteadyRadius));
        }

        public void OnBloomComplete()
        {
            gameEndState.OnBloomComplete();
        }

        private void OnTriggerStay(Collider other)
        {
            Transform bloomable = other.GetComponent<BloomView>().TargetBloomable;

            if (bloomable.localScale.x > 0.99f)
            {
                return;
            }

            bloomable.localScale = Vector3.Lerp(
                bloomable.localScale,
                fullScale,
                Time.deltaTime * bloomSpeed
            );
        }

        private void Awake()
        {
            fullScale = new Vector3(1, 1, 1);
            sphereCollider = GetComponent<SphereCollider>();
            initialRadius = sphereCollider.radius;
        }
    }
}
