using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class CameraAnchorManager : ITickable
    {
        private readonly Settings settings;
        private readonly CameraAnchorView anchorView;
        private readonly CameraOriginView originView;
        private readonly PlayerManager playerManager;

        public CameraAnchorManager(
                Settings settings,
                CameraAnchorView anchorView,
                CameraOriginView originView,
                PlayerManager playerManager)
        {
            this.settings = settings;
            this.anchorView = anchorView;
            this.originView = originView;
            this.playerManager = playerManager;
        }

        public void Tick()
        {
            Vector3 center = CenterPoint(playerManager.PlayerViews);

            anchorView.transform.position = Vector3.Lerp(
                anchorView.transform.position,
                center,
                settings.centeringSpeed * Time.deltaTime
            );

            Vector3 originProjection = new Vector3(
                originView.transform.position.x,
                anchorView.transform.position.y,
                originView.transform.position.z
            );
            Quaternion rotation = Quaternion.LookRotation(originProjection - center);

            anchorView.transform.rotation = Quaternion.Lerp(
                anchorView.transform.rotation,
                rotation,
                settings.rotationSpeed * Time.deltaTime
            );
        }

        private Vector3 CenterPoint(List<PlayerView> playerViews)
        {
            if (playerViews.Count == 0)
            {
                return Vector3.zero;
            }
            else if (playerViews.Count == 1)
            {
                return playerViews[0].transform.position;
            }

            Vector3 localCenter = Vector3.zero;
            foreach (PlayerView playerView in playerViews)
            {
                localCenter += playerView.transform.position;
            }

            return localCenter / playerViews.Count;
        }

        [Serializable]
        public class Settings
        {
            public float centeringSpeed;
            public float rotationSpeed;
        }
    }
}
