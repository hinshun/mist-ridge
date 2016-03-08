using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenject;

namespace MistRidge
{
    public class PlayerController : IInitializable, ITickable
    {
        private readonly Settings settings;
        private readonly PlayerView playerView;
        private readonly Collidable defaultCollidable;

        public PlayerStateMachine playerStateMachine;

        private float deltaTime;
        private float fixedDeltaTime;

        private float maxCollisionSphereRadius = 0f;
        private CollisionSphere headSphere;
        private CollisionSphere feetSphere;
        private ReadOnlyCollection<CollisionSphere> collisionSpheres;
        private readonly CollisionSphere.Factory collisionSphereFactory;
        private List<Collision> collisions;

        private Vector3 lastGroundPosition;
        private Grounding grounding;

        private bool isClamping = true;
        private bool isSlopeLimiting = true;

        private int ignoreCollisionLayer;
        private List<Collider> ignoredColliders;
        private Stack<ColliderInfo> ignoredColliderStack;

        public PlayerController(
                Settings settings,
                Grounding grounding,
                PlayerView playerView,
                Collidable defaultCollidable,
                CollisionSphere.Factory collisionSphereFactory)
        {
            this.settings = settings;
            this.grounding = grounding;
            this.playerView = playerView;
            this.defaultCollidable = defaultCollidable;
            this.collisionSphereFactory = collisionSphereFactory;
        }

        public float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

        public CollisionSphere HeadSphere
        {
            get
            {
                return headSphere;
            }
        }

        public CollisionSphere FeetSphere
        {
            get
            {
                return feetSphere;
            }
        }

        public bool IsClamping
        {
            get
            {
                return isClamping;
            }
            set
            {
                isClamping = value;
            }
        }

        public bool IsSlopeLimiting
        {
            get
            {
                return isSlopeLimiting;
            }
            set
            {
                isSlopeLimiting = value;
            }
        }

        public void Initialize()
        {
            CreateCollisionSpheres();

            ignoreCollisionLayer = LayerMask.NameToLayer(settings.ignoreCollisionLayerName);
            ignoredColliders = new List<Collider>();
            ignoredColliders.Add(playerView.Collider);
            ignoredColliderStack = new Stack<ColliderInfo>();

            collisions = new List<Collision>();

            fixedDeltaTime = 1f / settings.fixedUpdatesPerSecond;

            playerView.DrawGizmos += OnDrawGizmos;
        }

        public void Tick()
        {
            if (!settings.enableFixedTimeStep)
            {
                deltaTime = Time.deltaTime;
                Step();
            }
            else
            {
                float remainingDelta = Time.deltaTime;

                while (remainingDelta > fixedDeltaTime)
                {
                    deltaTime = fixedDeltaTime;
                    Step();
                    remainingDelta -=  fixedDeltaTime;
                }

                if (remainingDelta > 0)
                {
                    deltaTime = remainingDelta;
                    Step();
                }
            }
        }

        public bool EarlyAcquiringGround()
        {
            return grounding.IsGrounded(feetSphere, false, 1f);
        }

        public bool AcquiringGround()
        {
            return grounding.IsGrounded(feetSphere, false, 0.01f);
        }

        public bool MaintainingGround()
        {
            return grounding.IsGrounded(feetSphere, true, 0.5f);
        }

        public void ProbeGround()
        {
            PushIgnoredColliders();
            grounding.ProbeGround(feetSphere);
            PopIgnoredColliders();
        }

        private void Step()
        {
            Vector3 groundingDifference = grounding.Position - lastGroundPosition;

            if (isClamping && groundingDifference != Vector3.zero)
            {
                playerView.Position += groundingDifference;
            }

            Vector3 initialPosition = playerView.Position;

            ProbeGround();

            playerView.Position += settings.Debug.forceMovement * deltaTime;

            playerStateMachine.Tick();

            RecursivePushback();

            ProbeGround();

            if (isSlopeLimiting)
            {
                SlopeLimit(initialPosition);
            }

            ProbeGround();

            if (isClamping)
            {
                ClampToGround();
                lastGroundPosition = grounding.Position;
            }

            grounding.DebugGround();
        }

        private void SlopeLimit(Vector3 initialPosition)
        {
            Vector3 groundNormal = grounding.PrimaryNormal;
            float groundAngle = Vector3.Angle(groundNormal, playerView.Up);

            if (groundAngle > grounding.Collidable.SlopeLimit)
            {
                Vector3 absoluteMoveDirection = Math3d.ProjectVectorOnPlane(
                    groundNormal,
                    playerView.Position - initialPosition
                );

                Vector3 downslopeDirection = CollisionMath.DownslopeDirection(groundNormal, playerView.Down);
                float slopeAngle = Vector3.Angle(absoluteMoveDirection, downslopeDirection);

                if (slopeAngle <= settings.slopeLimitUpperBoundAngle)
                {
                    return;
                }

                Vector3 resolvedPosition = Math3d.ProjectPointOnLine(
                    initialPosition,
                    Vector3.Cross(groundNormal, playerView.Down),
                    playerView.Position
                );
                Vector3 direction = Math3d.ProjectVectorOnPlane(
                    groundNormal,
                    resolvedPosition - playerView.Position
                );

                RaycastHit hit;

                if (Physics.CapsuleCast(
                        feetSphere.Position,
                        headSphere.Position,
                        maxCollisionSphereRadius,
                        direction.normalized,
                        out hit,
                        direction.magnitude,
                        settings.walkableLayerMask
                    ))
                {
                    playerView.Position += downslopeDirection * hit.distance;
                }
                else
                {
                    playerView.Position += direction;
                }
            }
        }

        private void ClampToGround()
        {
            playerView.Position -= playerView.Up * grounding.PrimaryDistance;
        }

        private void RecursivePushback()
        {
            collisions.Clear();
            RecursivePushback(0);
        }

        private void RecursivePushback(int depth)
        {
            PushIgnoredColliders();

            bool hasContact = false;

            foreach (CollisionSphere collisionSphere in collisionSpheres)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    collisionSphere.Position,
                    collisionSphere.Radius,
                    settings.walkableLayerMask
                );

                foreach (Collider collider in colliders)
                {
                    if (collider.isTrigger)
                    {
                        continue;
                    }

                    Vector3 contactPoint = SuperCollider.ClosestPointOnSurface(
                        collider,
                        collisionSphere.Position,
                        collisionSphere.Radius
                    );

                    if (contactPoint != Vector3.zero)
                    {
                        if (settings.Debug.showPushbackMessages)
                        {
                            DebugDraw.DrawMarker(contactPoint, 2.0f, Color.cyan, 0.0f, false);
                        }

                        Vector3 contactDirection = contactPoint - collisionSphere.Position;

                        if (contactDirection != Vector3.zero)
                        {
                            int cachedLayer = collider.gameObject.layer;

                            collider.gameObject.layer = ignoreCollisionLayer;

                            Ray ray = new Ray(collisionSphere.Position, contactDirection.normalized);
                            bool facingNormal = Physics.SphereCast(
                                ray,
                                settings.epsilon,
                                contactDirection.magnitude + settings.epsilon,
                                1 << ignoreCollisionLayer
                            );

                            collider.gameObject.layer = cachedLayer;

                            if (facingNormal)
                            {
                                if (Vector3.Distance(collisionSphere.Position, contactPoint) < collisionSphere.Radius)
                                {
                                    contactDirection = -contactDirection.normalized * (collisionSphere.Radius - contactDirection.magnitude);
                                }
                                else
                                {
                                    // A previously resolved collision has had a
                                    // side effect that moved us outside the
                                    // collider
                                    continue;
                                }
                            }
                            else
                            {
                                contactDirection = contactDirection.normalized * (collisionSphere.Radius + contactDirection.magnitude);
                            }

                            hasContact = true;

                            playerView.Position += contactDirection;

                            collider.gameObject.layer = ignoreCollisionLayer;

                            RaycastHit normalHit;

                            Ray normalRay = new Ray(
                                collisionSphere.Position + contactDirection,
                                contactPoint - collisionSphere.Position - contactDirection
                            );

                            Physics.SphereCast(
                                normalRay,
                                settings.epsilon,
                                out normalHit,
                                1 << ignoreCollisionLayer
                            );

                            collider.gameObject.layer = cachedLayer;

                            Collidable collidable = collider.gameObject.GetComponent<Collidable>();

                            if (collidable == null)
                            {
                                collidable = defaultCollidable;
                            }

                            Collision collision = new Collision()
                            {
                                collisionSphere = collisionSphere,
                                collidable = collidable,
                                gameObject = collider.gameObject,
                                point = contactPoint,
                                normal = normalHit.normal
                            };

                            collisions.Add(collision);
                        }
                    }
                }
            }

            PopIgnoredColliders();

            if (depth < settings.maxPushbackIterations && hasContact)
            {
                RecursivePushback(depth + 1);
            }
        }

        private void PushIgnoredColliders()
        {
            foreach(Collider ignoredCollider in ignoredColliders)
            {
                ColliderInfo colliderInfo = new ColliderInfo(ignoredCollider);
                ignoredColliderStack.Push(colliderInfo);

                ignoredCollider.gameObject.layer = ignoreCollisionLayer;
            }
        }

        private void PopIgnoredColliders()
        {
            while (ignoredColliderStack.Count > 0)
            {
                ColliderInfo colliderInfo = ignoredColliderStack.Pop();
                colliderInfo.collider.gameObject.layer = colliderInfo.layer;
            }
        }

        private void CreateCollisionSpheres()
        {
            List<CollisionSphere> newCollisionSpheres = new List<CollisionSphere>();

            foreach(CollisionSphereRequest request in settings.collisionSphereRequests)
            {
                maxCollisionSphereRadius = Mathf.Max(maxCollisionSphereRadius, request.radius);

                CollisionSphere collisionSphere = collisionSphereFactory.Create(playerView, request);
                newCollisionSpheres.Add(collisionSphere);
            }

            if (newCollisionSpheres.Count == 0)
            {
                maxCollisionSphereRadius = settings.defaultCollisionSphereRequest.radius;

                CollisionSphere collisionSphere = collisionSphereFactory.Create(playerView, settings.defaultCollisionSphereRequest);
                newCollisionSpheres.Add(collisionSphere);
            }

            collisionSpheres = new ReadOnlyCollection<CollisionSphere>(newCollisionSpheres);
            feetSphere = collisionSpheres[0];
            headSphere = collisionSpheres[collisionSpheres.Count - 1];
        }

        private void OnDrawGizmos()
        {
            if (settings.Debug.showCollisionSpheres)
            {
                foreach (CollisionSphere collisionSphere in collisionSpheres)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(collisionSphere.Position, collisionSphere.Radius);
                }
            }
        }

        [Serializable]
        public class Settings
        {
            public DebugSettings Debug;
            public LayerMask walkableLayerMask;
            public string ignoreCollisionLayerName;
            public CollisionSphereRequest defaultCollisionSphereRequest;
            public List<CollisionSphereRequest> collisionSphereRequests;

            public float epsilon;

            public bool enableFixedTimeStep;
            public int fixedUpdatesPerSecond;

            public int maxPushbackIterations;

            public float slopeLimitUpperBoundAngle;

            [Serializable]
            public class DebugSettings
            {
                public Vector3 forceMovement;
                public bool showPushbackMessages;
                public bool showCollisionSpheres;
            }
        }
    }
}
