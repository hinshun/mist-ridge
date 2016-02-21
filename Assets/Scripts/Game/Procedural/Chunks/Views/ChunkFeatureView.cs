using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class ChunkFeatureView : ChunkChildView
    {
        [SerializeField]
        private PlatformBaseConfig platformBaseConfig = new PlatformBaseConfig()
        {
            spawnCenter = true,
            spawnTop = true,
            spawnTopLeft = true,
            spawnTopRight = true,
            spawnBottom = true,
            spawnBottomLeft = true,
            spawnBottomRight = true,
        };

        public PlatformBaseConfig PlatformBaseConfig
        {
            get
            {
                return platformBaseConfig;
            }
        }
    }
}
