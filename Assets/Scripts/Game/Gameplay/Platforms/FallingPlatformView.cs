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

        private BoxCollider boxCollider;

        private Hashtable fallHashtable;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            iTween.MoveTo(gameObject, fallHashtable);
        }

        private void FallEnd()
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();

            fallHashtable = new Hashtable();
            fallHashtable.Add("delay", delay);
            fallHashtable.Add("time", time);
            fallHashtable.Add("position", Position - (Vector3.up * dropDistance));
            fallHashtable.Add("oncomplete", "FallEnd");
            fallHashtable.Add("oncompletetarget", gameObject);
        }
    }
}
