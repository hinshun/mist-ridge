using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Grounding
    {
        private readonly Settings settings;
        private readonly PlayerView playerView;
        private readonly Collidable defaultCollidable;

        private Transform transform;

        private GroundHit primaryGround;
        private GroundHit nearGround;
        private GroundHit farGround;
        private GroundHit stepGround;
        private GroundHit flushGround;

        private Collidable collidable;

        private CollisionSphere collisionSphere;

        public Grounding(
                Settings settings,
                PlayerView playerView,
                Collidable defaultCollidable,
                CollisionSphere collisionSphere)
        {
            this.settings = settings;
            this.playerView = playerView;
            this.defaultCollidable = defaultCollidable;
            this.collisionSphere = collisionSphere;
        }

        public Vector3 PrimaryNormal
        {
            get
            {
                return primaryGround.normal;
            }
        }

        public float PrimaryDistance
        {
            get
            {
                return primaryGround.distance;
            }
        }

        public Vector3 Position
        {
            get
            {
                if (transform == null)
                {
                    return Vector3.zero;
                }

                return transform.position;
            }
        }

        public Collidable Collidable
        {
            get
            {
                return collidable;
            }
        }

        /*
         * Scans surface below controller for ground. Follows up the initial
         * scan with subsequent scans to handle different edge cases.
         */
        public void ProbeGround()
        {
            ResetGrounds();

            Vector3 origin = collisionSphere.Position + (playerView.Up * settings.tolerance);

            // Reduce radius to avoid failing SphereCast due to wall clipping
            float smallerRadius = collisionSphere.Radius - (settings.tolerance * settings.tolerance);

            RaycastHit hit;

            if (SphereCast(origin, smallerRadius, playerView.Down, out hit))
            {
                collidable = GetCollidable(hit);
                transform = hit.transform;

                SimulateSphereCast(hit.normal, out hit);

                primaryGround = new GroundHit(hit);

                // If we are standing on a perfectly flat surface, we cannot be
                // either on an edge, on a slope, or stepping off a ledge
                Vector3 surface = Math3d.ProjectPointOnPlane(playerView.Up, playerView.Position, hit.point);

                if (Vector3.Distance(surface, playerView.Position) < settings.epsilon)
                {
                    return;
                }

                // Now the collision is on an edge, so normals of the two faces
                // on either side of the edge are computed and stored in nearHit
                // and farHit
                HandleEdgeCollision(hit);
            }
            // If initial SphereCast fails, which is likely due to controller
            // clipping a wall, fallback to raycast simulating SphereCast data
            else if (Raycast(origin, playerView.Down, out hit))
            {
                HandleClippingCollision(hit);
            }
            else
            {
                Debug.LogError("No ground found below player");
            }
        }

        public bool IsGrounded(bool currentlyGrounded, float distance)
        {
            if (!primaryGround.isFound || primaryGround.distance > distance)
            {
                return false;
            }

            if (farGround.isFound)
            {
                // Check if flushing against wall
                if (IsGroundStandable(farGround))
                {
                    if (flushGround.isFound
                        && IsGroundStandable(flushGround)
                        && flushGround.distance < distance)
                    {
                        return true;
                    }

                    return false;
                }

                // Check if at edge of ledge, or high angle slope
                if (!OnSteadyGround(farGround.normal, primaryGround.point))
                {
                    if (nearGround.isFound
                        && nearGround.distance < distance
                        && IsGroundStandable(nearGround)
                        && !OnSteadyGround(nearGround.normal, nearGround.point))
                    {
                        return true;
                    }

                    if (stepGround.isFound
                        && stepGround.distance < distance
                        && IsGroundStandable(stepGround))
                    {
                        return true;
                    }

                    return false;
                }
            }

            return true;
        }

        public void DebugGround()
        {
            if (settings.Debug.Primary.showNormal)
            {
                DebugGround(primaryGround, settings.Debug.Primary.color);
            }

            if (settings.Debug.Near.showNormal)
            {
                DebugGround(nearGround, settings.Debug.Near.color);
            }

            if (settings.Debug.Far.showNormal)
            {
                DebugGround(farGround, settings.Debug.Far.color);
            }

            if (settings.Debug.Flush.showNormal)
            {
                DebugGround(flushGround, settings.Debug.Flush.color);
            }

            if (settings.Debug.Step.showNormal)
            {
                DebugGround(stepGround, settings.Debug.Step.color);
            }
        }

        private void DebugGround(GroundHit groundHit, Color color)
        {
            if (groundHit.isFound)
            {
                DebugDraw.DrawVector(
                    groundHit.point,
                    groundHit.normal,
                    settings.Debug.raySize,
                    settings.Debug.markerSize,
                    color,
                    0,
                    false
                );
            }
        }

        /*
         * Checks if the ground below player is "steady", or that the standing
         * surface is not too extreme of an ledge, allowing player to smoothly
         * fall off surfaces and not hang on edge of ledges
         */
        private bool OnSteadyGround(Vector3 normal, Vector3 point)
        {
            float angle = Vector3.Angle(normal, playerView.Up);

            float angleRatio = angle / settings.upperBoundAngle;

            float distanceRatio = Mathf.Lerp(
                settings.minPercentFromCenter,
                settings.maxPercentFromCenter,
                angleRatio
            );

            Vector3 projection = Math3d.ProjectPointOnPlane(playerView.Up, playerView.Position, point);

            float distanceFromCenter = Vector3.Distance(projection, playerView.Position);

            return distanceFromCenter <= distanceRatio * collisionSphere.Radius;
        }

        private void HandleEdgeCollision(RaycastHit hit)
        {
            Vector3 towardCenter = Math3d.ProjectVectorOnPlane(
                playerView.Up,
                (playerView.Position - hit.point).normalized * settings.epsilon
            );

            Quaternion planeAwayRotation = Quaternion.AngleAxis(
                settings.edgeCollisionRotateDegree,
                Vector3.Cross(towardCenter, playerView.Up)
            );

            Vector3 awayCenter = planeAwayRotation * -towardCenter;

            Vector3 nearPoint = hit.point + towardCenter + (playerView.Up * settings.epsilon);
            Vector3 farPoint = hit.point + (awayCenter * settings.edgeCollisionFarPointMultiplier);

            RaycastHit nearHit;
            RaycastHit farHit;

            Raycast(nearPoint, playerView.Down, out nearHit);
            Raycast(farPoint, playerView.Down, out farHit);

            nearGround = new GroundHit(nearHit);
            farGround = new GroundHit(farHit);

            // If we are standing on surface that should be counted as a
            // wall, attempt to flush against it on the ground
            if (Vector3.Angle(hit.normal, playerView.Up) > collidable.StandAngle)
            {
                FlushGround(hit);
            }

            // If we are standing on a ledge then face the nearest center of
            // the player view, which should be steep enough to be counted as
            // a wall. Retrieve the ground it is connected to at its base, if
            // there is one.
            if ((Vector3.Angle(nearHit.normal, playerView.Up) > collidable.StandAngle) || (nearHit.distance > settings.tolerance))
            {
                Collidable nearCollidable = GetCollidable(nearHit);

                if (Vector3.Angle(nearHit.normal, playerView.Up) > nearCollidable.StandAngle)
                {
                    Vector3 downslopeDirection = CollisionMath.DownslopeDirection(nearHit.normal, playerView.Down);

                    RaycastHit stepHit;

                    if (Raycast(nearPoint, downslopeDirection, out stepHit))
                    {
                        stepGround = new GroundHit(stepHit);
                    }
                }
                else
                {
                    stepGround = new GroundHit(nearHit);
                }
            }
        }

        private void FlushGround(RaycastHit hit)
        {
            Vector3 downslopeDirection = CollisionMath.DownslopeDirection(hit.normal, playerView.Down);

            Vector3 flushOrigin = hit.point + (hit.normal * settings.epsilon);

            RaycastHit flushHit;

            if (Raycast(flushOrigin, downslopeDirection, out flushHit))
            {
                RaycastHit sphereCastHit;

                if (SimulateSphereCast(flushHit.normal, out sphereCastHit))
                {
                    flushGround = new GroundHit(sphereCastHit);
                }
                else
                {
                    Debug.LogError("Failed to flush against ground");
                }
            }
        }

        private void HandleClippingCollision(RaycastHit hit)
        {
            collidable = GetCollidable(hit);
            transform = hit.transform;

            RaycastHit sphereCastHit;

            if (SimulateSphereCast(hit.normal, out sphereCastHit))
            {
                primaryGround = new GroundHit(sphereCastHit);
            }
            else
            {
                primaryGround = new GroundHit(hit);
            }
        }

        /*
         * Provides raycast data based on where a SphereCast would have contacted
         * the specified normal.
         * Raycasting downwards from a point along the controller's bottom sphere,
         * based on the provided normal.
         */
        private bool SimulateSphereCast(Vector3 groundNormal, out RaycastHit hit)
        {
            float groundAngle = Vector3.Angle(groundNormal, playerView.Up) * Mathf.Deg2Rad;

            Vector3 secondaryOrigin = playerView.Position + (playerView.Up * settings.tolerance);

            if (!Mathf.Approximately(groundAngle, 0))
            {
                float horizontal = Mathf.Sin(groundAngle) * collisionSphere.Radius;
                float vertical = (1f - Mathf.Cos(groundAngle)) * collisionSphere.Radius;

                Vector3 upslopeDirection = -CollisionMath.DownslopeDirection(groundNormal, playerView.Down);

                Vector3 horizontalDirection = Math3d.ProjectVectorOnPlane(playerView.Up, upslopeDirection).normalized;
                secondaryOrigin += horizontalDirection * horizontal + playerView.Up * vertical;
            }

            if (SphereCast(secondaryOrigin, settings.epsilon, playerView.Down, out hit))
            {
                hit.distance -= settings.tolerance;
                return true;
            }

            return false;
        }

        private void ResetGrounds()
        {
            primaryGround = new GroundHit();
            nearGround = new GroundHit();
            farGround = new GroundHit();
            flushGround = new GroundHit();
            stepGround = new GroundHit();
        }

        private bool IsGroundStandable(GroundHit groundHit)
        {
            return Vector3.Angle(groundHit.normal, playerView.Up) < collidable.StandAngle;
        }

        private Collidable GetCollidable(RaycastHit hit)
        {
            Collidable hitCollidable = hit.collider.gameObject.GetComponent<Collidable>();

            if (hitCollidable == null)
            {
                return defaultCollidable;
            }

            return hitCollidable;
        }

        private bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
        {
            return Physics.Raycast(
                origin,
                direction,
                out hitInfo,
                maxDistance: Mathf.Infinity,
                layerMask: settings.walkableLayerMask
            );
        }

        private bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo)
        {
            return Physics.SphereCast(
                origin,
                radius,
                direction,
                out hitInfo,
                maxDistance: Mathf.Infinity,
                layerMask: settings.walkableLayerMask
            );
        }

        public class Factory : Factory<PlayerView, CollisionSphere, Grounding>
        {
        }

        [Serializable]
        public class Settings
        {
            public DebugSettings Debug;

            public LayerMask walkableLayerMask;

            public float upperBoundAngle;
            public float maxPercentFromCenter;
            public float minPercentFromCenter;

            public float edgeCollisionRotateDegree;
            public float edgeCollisionFarPointMultiplier;

            public float tolerance;
            public float epsilon;

            [Serializable]
            public class DebugSettings
            {
                public GroundDebugConfig Primary;
                public GroundDebugConfig Near;
                public GroundDebugConfig Far;
                public GroundDebugConfig Flush;
                public GroundDebugConfig Step;

                public float raySize;
                public float markerSize;

                [Serializable]
                public class GroundDebugConfig
                {
                    public bool showNormal;
                    public Color color;
                }
            }
        }
    }
}
