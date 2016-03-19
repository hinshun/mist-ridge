using UnityEngine;
using System;
using System.Collections;

namespace MistRidge
{
    public class EmoteView : PoolInstanceView
    {
        [SerializeField]
        private float scaleTime;

        [SerializeField]
        private Vector3 initialScale;

        [SerializeField]
        private float fadeTime;

        private SpriteRenderer spriteRenderer;
        private Hashtable fadeHashtable;

        private Hashtable scaleHashtable;
        private Vector3 finalScale;

        public override void OnPoolInstanceReuse()
        {
            LocalScale = initialScale;
            spriteRenderer.color = Color.white;
            iTween.ScaleTo(gameObject, scaleHashtable);
        }

        private void Awake()
        {
            finalScale = LocalScale;
            spriteRenderer = GetComponent<SpriteRenderer>();

            scaleHashtable = new Hashtable();
            scaleHashtable.Add("scale", finalScale);
            scaleHashtable.Add("time", scaleTime);
            scaleHashtable.Add("easetype", iTween.EaseType.easeOutBounce);
            scaleHashtable.Add("oncomplete", "FadeOut");
            scaleHashtable.Add("oncompletetarget", gameObject);

            fadeHashtable = new Hashtable();
            fadeHashtable.Add("from", Color.white);
            fadeHashtable.Add("to", Color.clear);
            fadeHashtable.Add("time", fadeTime);
            fadeHashtable.Add("onupdate", "Fade");
            fadeHashtable.Add("onupdatetarget", gameObject);
            fadeHashtable.Add("oncomplete", "FadeFinish");
            fadeHashtable.Add("oncompletetarget", gameObject);
        }

        private void Fade(Color color)
        {
            spriteRenderer.color = color;
        }

        private void FadeOut()
        {
            iTween.ValueTo(gameObject, fadeHashtable);
        }

        private void FadeFinish()
        {
            Destroy();
        }
    }
}
