using UnityEngine;
using System;
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

        private bool canJump;
        private bool canPickupItems;
        private bool canControl;
        private Animator animator;
        private ItemPickupSignal.Trigger itemPickupTrigger;
        private ItemEffectSignal.Trigger itemEffectTrigger;
        private CheckpointSignal.Trigger checkpointTrigger;
        private Dictionary<CheckpointView, bool> checkpointsVisited;

        [PostInject]
        public void Init(
                ItemPickupSignal.Trigger itemPickupTrigger,
                ItemEffectSignal.Trigger itemEffectTrigger,
                CheckpointSignal.Trigger checkpointTrigger)
        {
            this.itemPickupTrigger = itemPickupTrigger;
            this.itemEffectTrigger = itemEffectTrigger;
            this.checkpointTrigger = checkpointTrigger;
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

        public bool CanControl
        {
            get
            {
                return canControl;
            }
            set
            {
                canControl = value;
            }
        }

        public void OnDrawGizmos()
        {
            DrawGizmos();
        }

        public void OnItemPickup(ItemType itemType)
        {
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

            canJump = true;
            canPickupItems = true;
            canControl = true;

            IsDustTrailEmitting = false;
        }
    }
}
