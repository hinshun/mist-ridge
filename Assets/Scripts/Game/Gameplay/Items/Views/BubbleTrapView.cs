using UnityEngine;
using System;
using System.Collections;

namespace MistRidge
{
    public class BubbleTrapView : PoolInstanceView
    {
        [SerializeField]
        private float throwTime;

        [SerializeField]
        private float throwHeight;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float height;

        [SerializeField]
        private Vector3 initialScale;

        private Hashtable moveHashtable;
        private Hashtable scaleHashtable;
        private Hashtable trappedHashtable;
        private Hashtable destroyHashtable;
        private bool activated;
        private bool trapped;
        private PlayerView playerView;
        private Vector3 finalScale;

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
            trapped = false;
            playerView = null;
            LocalScale = initialScale;
        }

        public void Land(Vector3 landingPosition)
        {
            Vector3[] path = new Vector3[] {
                (transform.position + landingPosition) / 2 + (Vector3.up * throwHeight),
                landingPosition
            };
            moveHashtable["path"] = path;
            trappedHashtable["position"] = landingPosition + (Vector3.up * height);
            iTween.MoveTo(gameObject, moveHashtable);
            iTween.ScaleTo(gameObject, scaleHashtable);
        }

        private void SetupTrap()
        {
            activated = true;
        }

        private void Pop()
        {
            activated = false;
            trapped = false;

            if (playerView != null)
            {
                playerView.Parent = Parent;
                playerView.BubbleTrapRelease();
            }

            Destroy();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!activated || !other.CompareTag("Player"))
            {
                return;
            }

            if (trapped)
            {
                PlayerView otherPlayerView = other.GetComponent<PlayerView>();
                if (otherPlayerView == playerView)
                {
                    return;
                }

                iTween.ScaleTo(gameObject, destroyHashtable);
                return;
            }

            playerView = other.GetComponent<PlayerView>();
            if (playerView == null)
            {
                return;
            }

            playerView.BubbleTrapped();
            trapped = true;

            Parent = playerView.Parent;
            playerView.Parent = transform;
            playerView.LocalPosition = Vector3.zero;

            iTween.MoveTo(gameObject, trappedHashtable);
        }

        private void Awake()
        {
            finalScale = LocalScale;

            moveHashtable = new Hashtable();
            moveHashtable.Add("time", throwTime);
            moveHashtable.Add("oncomplete", "SetupTrap");
            moveHashtable.Add("oncompletetarget", gameObject);

            scaleHashtable = new Hashtable();
            scaleHashtable.Add("time", throwTime);
            scaleHashtable.Add("scale", finalScale);

            trappedHashtable = new Hashtable();
            trappedHashtable.Add("time", duration);
            trappedHashtable.Add("oncomplete", "Pop");
            trappedHashtable.Add("oncompletetarget", gameObject);

            destroyHashtable = new Hashtable();
            destroyHashtable.Add("time", 0.1f);
            destroyHashtable.Add("scale", finalScale * 1.1f);
            destroyHashtable.Add("oncomplete", "Pop");
            destroyHashtable.Add("oncompletetarget", gameObject);
        }
    }
}
