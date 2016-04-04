using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class PlayerView : MonoView
    {
        public event Action DrawGizmos = delegate {};

        [SerializeField]
        private Collider collider;

        [SerializeField]
        private Transform meshTransform;

        [SerializeField]
        private Transform handTransform;

        [SerializeField]
        private Transform emoteTransform;

        [SerializeField]
        private float itemFlashTime;

        [SerializeField]
        private List<SkinnedMeshRenderer> meshRenderers;

        [SerializeField]
        private ParticleSystem dustTrail;

        [SerializeField]
        private ParticleSystem dustLand;

        [SerializeField]
        private int dustJumpParticleCount;

        [SerializeField]
        private int dustLandParticleCount;

        [SerializeField]
        private ParticleSystem dustFreefallLand;

        [SerializeField]
        private int dustFreefallLandParticleCount;

        [SerializeField]
        private List<ParticleSystem> afterImages;

        [SerializeField]
        private int afterImageParticleCount;

        [SerializeField]
        private SpriteRenderer playerCircle;

        private Hashtable itemFlashInHashtable;
        private Hashtable itemFlashOutHashtable;

        private bool canJump;
        private bool canPickupItems;

        private Animator animator;
        private ItemPickupSignal.Trigger itemPickupTrigger;
        private ItemEffectSignal.Trigger itemEffectTrigger;
        private CheckpointSignal.Trigger checkpointTrigger;
        private CinematicSignal.Trigger cinematicTrigger;
        private PlayerControlSignal.Trigger playerControlTrigger;
        private Dictionary<CheckpointView, bool> checkpointsVisited;

        [PostInject]
        public void Init(
                ItemPickupSignal.Trigger itemPickupTrigger,
                ItemEffectSignal.Trigger itemEffectTrigger,
                CheckpointSignal.Trigger checkpointTrigger,
                CinematicSignal.Trigger cinematicTrigger,
                PlayerControlSignal.Trigger playerControlTrigger)
        {
            this.itemPickupTrigger = itemPickupTrigger;
            this.itemEffectTrigger = itemEffectTrigger;
            this.checkpointTrigger = checkpointTrigger;
            this.cinematicTrigger = cinematicTrigger;
            this.playerControlTrigger = playerControlTrigger;
        }

        public Vector3 Forward
        {
            get
            {
                return meshTransform.forward;
            }
        }

        public Collider Collider
        {
            get
            {
                return collider;
            }
        }

        public Animator Animator
        {
            get
            {
                return animator;
            }
        }

        public bool IsDustTrailEmitting
        {
            get
            {
                ParticleSystem.EmissionModule emission = dustTrail.emission;
                return emission.enabled;
            }
            set
            {
                ParticleSystem.EmissionModule emission = dustTrail.emission;
                emission.enabled = value;
            }
        }

        public Transform MeshTransform
        {
            get
            {
                return meshTransform;
            }
        }

        public Transform HandTransform
        {
            get
            {
                return handTransform;
            }
        }

        public bool CanJump
        {
            get
            {
                return canJump;
            }
            set
            {
                canJump = value;
            }
        }

        public bool CanPickupItems
        {
            get
            {
                return canPickupItems;
            }
            set
            {
                canPickupItems = value;
            }
        }

        public SpriteRenderer PlayerCircle
        {
            get
            {
                return playerCircle;
            }
        }

        public void OnDrawGizmos()
        {
            DrawGizmos();
        }

        public void OnItemPickup(ItemType itemType)
        {
            iTween.ValueTo(gameObject, itemFlashInHashtable);
            itemPickupTrigger.Fire(itemType);
        }

        public void OnCheckpoint(CheckpointView checkpointView)
        {
            if (!checkpointsVisited.ContainsKey(checkpointView))
            {
                checkpointsVisited[checkpointView] = true;
                checkpointTrigger.Fire(checkpointView, this);
            }
        }

        public void PlayerAllowJump()
        {
            canJump = true;
        }

        public void PlayerGettingUp()
        {
            PlayerAllowJump();
            playerControlTrigger.Fire(this, true);
            cinematicTrigger.Fire(CinematicTransitionType.SlideOut);
        }

        public void PlayerJump()
        {
            dustTrail.Emit(dustJumpParticleCount);
            dustTrail.Play();
        }

        public void PlayerLand()
        {
            dustLand.Emit(dustLandParticleCount);
        }

        public void PlayerFreefallLand()
        {
            dustLand.Emit(dustFreefallLandParticleCount);
        }

        public void PlayerAfterImage()
        {
            foreach(ParticleSystem afterImage in afterImages)
            {
                afterImage.Emit(afterImageParticleCount);
            }
        }

        public void BubbleTrapped()
        {
            itemEffectTrigger.Fire(ItemType.BubbleTrap, ItemStage.Start);
        }

        public void BubbleTrapRelease()
        {
            itemEffectTrigger.Fire(ItemType.BubbleTrap, ItemStage.End);
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            checkpointsVisited = new Dictionary<CheckpointView, bool>();

            itemFlashInHashtable = new Hashtable();
            itemFlashInHashtable.Add("from", Color.black);
            itemFlashInHashtable.Add("to", Color.white);
            itemFlashInHashtable.Add("time", itemFlashTime);
            itemFlashInHashtable.Add("onupdate", "ItemFlash");
            itemFlashInHashtable.Add("onupdatetarget", gameObject);
            itemFlashInHashtable.Add("oncomplete", "ItemFlashOut");
            itemFlashInHashtable.Add("oncompletetarget", gameObject);

            itemFlashOutHashtable = new Hashtable();
            itemFlashOutHashtable.Add("from", Color.white);
            itemFlashOutHashtable.Add("to", Color.black);
            itemFlashOutHashtable.Add("time", itemFlashTime);
            itemFlashOutHashtable.Add("onupdate", "ItemFlash");
            itemFlashOutHashtable.Add("onupdatetarget", gameObject);

            canJump = true;
            canPickupItems = true;

            IsDustTrailEmitting = false;
        }

        private void ItemFlash(Color color)
        {
            foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetColor("_EmissionColor", color);
            }
        }

        private void ItemFlashOut()
        {
            iTween.ValueTo(gameObject, itemFlashOutHashtable);
        }
    }
}
