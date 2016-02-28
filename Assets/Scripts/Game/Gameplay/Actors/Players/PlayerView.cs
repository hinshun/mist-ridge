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
        private ReadOnlyCollection<Collider> readOnlyColliders;

        [PostInject]
        public void Init(ItemPickupSignal.Trigger itemPickupTrigger)
        {
            this.itemPickupTrigger = itemPickupTrigger;
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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            readOnlyColliders = new ReadOnlyCollection<Collider>(colliders);
        }
    }
}
