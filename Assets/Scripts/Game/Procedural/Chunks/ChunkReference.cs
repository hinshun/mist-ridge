using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Chunk Reference")]
    public class ChunkReference : ScriptableObject
    {
        [SerializeField]
        private Vector3 localScale;

        [SerializeField]
        private Mesh platformBaseMesh;

        private Vector3 forwardLeft;
        private Vector3 forwardRight;
        private Vector3 left;
        private Vector3 right;
        private Vector3 backwardLeft;
        private Vector3 backwardRight;

        private Vector3 northwest;
        private Vector3 northeast;
        private Vector3 west;
        private Vector3 east;
        private Vector3 southwest;
        private Vector3 southeast;

        public Vector3 LocalScale
        {
            get
            {
                return localScale;
            }
        }

        public Mesh PlatformBaseMesh
        {
            get
            {
                return platformBaseMesh;
            }
        }

        public Vector3 ForwardLeft
        {
            get
            {
                if (forwardLeft == Vector3.zero)
                {
                    forwardLeft = ScaledPosition(new Vector2(0, 2f));
                }
                return forwardLeft;
            }
        }

        public Vector3 ForwardRight
        {
            get
            {
                if (forwardRight == Vector3.zero)
                {
                    forwardRight = ScaledPosition(new Vector2(1.5f, 1f));
                }
                return forwardRight;
            }
        }

        public Vector3 Left
        {
            get
            {
                if (left == Vector3.zero)
                {
                    left = ScaledPosition(new Vector2(-1.5f, 1f));
                }
                return left;
            }
        }

        public Vector3 Center
        {
            get
            {
                return Vector3.zero;
            }
        }

        public Vector3 Right
        {
            get
            {
                if (right == Vector3.zero)
                {
                    right = ScaledPosition(new Vector2(1.5f, -1f));
                }
                return right;
            }
        }

        public Vector3 BackwardLeft
        {
            get
            {
                if (backwardLeft == Vector3.zero)
                {
                    backwardLeft = ScaledPosition(new Vector2(-1.5f, -1f));
                }
                return backwardLeft;
            }
        }

        public Vector3 BackwardRight
        {
            get
            {
                if (backwardRight == Vector3.zero)
                {
                    backwardRight = ScaledPosition(new Vector2(0, -2f));
                }
                return backwardRight;
            }
        }

        public Vector3 Northwest
        {
            get
            {
                if (northwest == Vector3.zero)
                {
                    northwest = ScaledPosition(new Vector2(-3f, 4f));
                }
                return northwest;
            }
        }

        public Vector3 Northeast
        {
            get
            {
                if (northeast == Vector3.zero)
                {
                    northeast = ScaledPosition(new Vector2(1.5f, 5f));
                }
                return northeast;
            }
        }

        public Vector3 West
        {
            get
            {
                if (west == Vector3.zero)
                {
                    west = ScaledPosition(new Vector2(-4.5f, -1f));
                }
                return west;
            }
        }

        public Vector3 East
        {
            get
            {
                return -west;
            }
        }

        public Vector3 Southwest
        {
            get
            {
                return -northeast;
            }
        }

        public Vector3 Southeast
        {
            get
            {
                return -northwest;
            }
        }

        private Vector3 ScaledPosition(Vector2 relativePosition)
        {
            return new Vector3(
                relativePosition.x * localScale.x,
                0,
                relativePosition.y * localScale.z * Mathf.Sqrt(3) / 2
            );
        }
    }
}
