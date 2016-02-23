using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class PlayerView : MonoBehaviour
    {
        public event Action DrawGizmos = delegate {};

        private ItemPickupSignal.Trigger itemPickupTrigger;

        [SerializeField]
        private List<Collider> colliders;

        [SerializeField]
        private Transform meshTransform;

        [SerializeField]
        private MeshRenderer meshRenderer;

        private ReadOnlyCollection<Collider> readOnlyColliders;

        [PostInject]
        public void Init(ItemPickupSignal.Trigger itemPickupTrigger)
        {
            this.itemPickupTrigger = itemPickupTrigger;
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 Up
        {
            get
            {
                return transform.up;
            }
        }

        public Vector3 Down
        {
            get
            {
                return -Up;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return transform.forward;
            }
        }

        public ReadOnlyCollection<Collider> Colliders
        {
            get
            {
                return readOnlyColliders;
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
            readOnlyColliders = new ReadOnlyCollection<Collider>(colliders);
        }
    }
}
