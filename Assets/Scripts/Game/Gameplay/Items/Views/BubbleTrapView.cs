using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class BubbleTrapView : PoolInstanceView
    {
        [SerializeField]
        private float throwTime;

        [SerializeField]
        private float throwHeight;

        [SerializeField]
        private float scaleTime;

        [SerializeField]
        private float popTime;

        [SerializeField]
        private Vector3 popScale;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float height;

        [SerializeField]
        private Vector3 initialScale;

        private bool activated;
        private bool trapped;
        private bool popping;
        private float moveId;

        private Hashtable moveHashtable;
        private Hashtable scaleHashtable;
        private Hashtable trappedHashtable;
        private Hashtable destroyHashtable;

        private PlayerView playerView;
        private Vector3 finalScale;

        private PlayerContainerView playerContainerView;

        [PostInject]
        public void Init(PlayerContainerView playerContainerView)
        {
            this.playerContainerView = playerContainerView;
        }

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
            popping = false;
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

            iTween.MoveTo(gameObject, moveHashtable);
            iTween.ScaleTo(gameObject, scaleHashtable);
        }

        public void Pop()
        {
            activated = false;
            trapped = false;
            popping = false;

            if (playerView != null)
            {
                playerView.Parent = playerContainerView.transform;
                playerView.LocalScale = new Vector3(1, 1, 1);
                playerView.BubbleTrapRelease();
            }

            LocalScale = initialScale;
            Remove();
        }

        public void OnTriggerStay(Collider other)
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

                popping = true;

                iTween.ScaleTo(gameObject, destroyHashtable);
                return;
            }

            playerView = other.GetComponent<PlayerView>();
            if (playerView == null || playerView.IsBubbleTrapped)
            {
                return;
            }

            iTween.StopByName(gameObject, "moveTween" + moveId);
            playerView.BubbleTrapped();
            trapped = true;

            playerView.Parent = transform;
            playerView.LocalPosition = Vector3.zero;

            trappedHashtable["position"] = Position + (Vector3.up * height);
            iTween.MoveTo(gameObject, trappedHashtable);
        }

        private void SetupTrap()
        {
            activated = true;
        }

        private void Awake()
        {
            finalScale = LocalScale;

            activated = false;
            trapped = false;
            popping = false;

            moveId = UnityEngine.Random.value;

            moveHashtable = new Hashtable();
            moveHashtable.Add("name", "moveTween" + moveId);
            moveHashtable.Add("time", throwTime);

            scaleHashtable = new Hashtable();
            scaleHashtable.Add("time", scaleTime);
            scaleHashtable.Add("scale", finalScale);
            scaleHashtable.Add("oncomplete", "SetupTrap");

            trappedHashtable = new Hashtable();
            trappedHashtable.Add("time", duration);
            trappedHashtable.Add("oncomplete", "Pop");

            destroyHashtable = new Hashtable();
            destroyHashtable.Add("time", popTime);
            destroyHashtable.Add("scale", popScale);
            destroyHashtable.Add("oncomplete", "Pop");
        }
    }
}
