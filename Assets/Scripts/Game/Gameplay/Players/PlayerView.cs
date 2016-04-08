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
        private ParticleSystem respawn;

        [SerializeField]
        private SpriteRenderer playerCircle;

        [SerializeField]
        private ParticleSystem itemUse;

        private Hashtable itemFlashInHashtable;
        private Hashtable itemFlashOutHashtable;

        private bool canUseItems;
        private bool canJump;
        private bool canPickupItems;
        private bool isBubbleTrapped;

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

        public bool IsBubbleTrapped
        {
            get
            {
                return isBubbleTrapped;
            }
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

        public ParticleSystem Respawn
        {
            get
            {
                return respawn;
            }
        }

        public ParticleSystem ItemUse
        {
            get
            {
                return itemUse;
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

        public bool CanUseItems
        {
            get
            {
                return canUseItems && !isBubbleTrapped;
            }
            set
            {
                canUseItems = value;
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
                playerControlTrigger.Fire(this, false);
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
            isBubbleTrapped = true;
            itemEffectTrigger.Fire(ItemType.BubbleTrap, ItemStage.Start);
        }

        public void BubbleTrapRelease()
        {
            isBubbleTrapped = false;
            itemEffectTrigger.Fire(ItemType.BubbleTrap, ItemStage.End);
        }

        private void Awake()
        {
            canUseItems = true;
            canJump = true;
            canPickupItems = true;
            isBubbleTrapped = false;
            IsDustTrailEmitting = false;

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
