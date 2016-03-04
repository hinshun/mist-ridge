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
        private List<Collider> colliders;

        [SerializeField]
        private Transform meshTransform;

        [SerializeField]
        private MeshRenderer meshRenderer;

        private bool canPickupItems;
        private Animator animator;
        private ItemPickupSignal.Trigger itemPickupTrigger;
        private CheckpointSignal.Trigger checkpointTrigger;
        private ReadOnlyCollection<Collider> readOnlyColliders;
        private Dictionary<CheckpointView, bool> checkpointsVisited;

        [PostInject]
        public void Init(
                ItemPickupSignal.Trigger itemPickupTrigger,
                CheckpointSignal.Trigger checkpointTrigger)
        {
            this.itemPickupTrigger = itemPickupTrigger;
            this.checkpointTrigger = checkpointTrigger;
        }

        public ReadOnlyCollection<Collider> Colliders
        {
            get
            {
                return readOnlyColliders;
            }
        }

        public Animator Animator
        {
            get
            {
                return animator;
            }
        }

        public Transform MeshTransform
        {
            get
            {
                return meshTransform;
            }
        }

        public MeshRenderer MeshRenderer
        {
            get
            {
                return meshRenderer;
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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            readOnlyColliders = new ReadOnlyCollection<Collider>(colliders);
            checkpointsVisited = new Dictionary<CheckpointView, bool>();
        }
    }
}
