using UnityEngine;
using System;
using System.Collections;

namespace MistRidge
{
    public class FallingPlatformView : MonoView
    {
        [SerializeField]
        private float delay;

        [SerializeField]
        private float time;

        [SerializeField]
        private float dropDistance;

        private Vector3 initialPosition;
        private Hashtable fallHashtable;
        private BoxCollider boxCollider;
        private MeshRenderer meshRenderer;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            iTween.ValueTo(gameObject, fallHashtable);
        }

        private void FallEnd(float time)
        {
            Position = Vector3.Lerp(
                initialPosition,
                initialPosition - (Vector3.up * dropDistance),
                time
            );

            meshRenderer.material.color = new Color(1, 1, 1, 1 - time);
        }

        private void Awake()
        {
            initialPosition = Position;
            boxCollider = GetComponent<BoxCollider>();
            meshRenderer = GetComponent<MeshRenderer>();

            fallHashtable = new Hashtable();
            fallHashtable.Add("from", 0);
            fallHashtable.Add("to", 1);
            fallHashtable.Add("time", time);
            fallHashtable.Add("delay", delay);
            fallHashtable.Add("oncomplete", "FallEnd");

            /* fallHashtable.Add("position", Position - (Vector3.up * dropDistance)); */
        }
    }
}
