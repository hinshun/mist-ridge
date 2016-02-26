using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class SpiralChunkPlacingStrategy : IChunkPlacingStrategy
    {
        private readonly ChunkReference chunkReference;
        private readonly int chunkCount;

        public SpiralChunkPlacingStrategy(
                ChunkManager chunkManager)
        {
            this.chunkReference = chunkManager.ChunkReference;
            this.chunkCount = chunkManager.ChunkCount;
        }

        public void Placement(ChunkView chunkView, ChunkConfig chunkConfig)
        {
            if (chunkConfig.chunkNum == 0)
            {
                chunkView.Rotation *= Quaternion.AngleAxis(
                    120,
                    chunkView.Up
                );
                chunkView.Position = Vector3.zero + Altitude(chunkConfig);
                return;
            }

            int depth = Mathf.FloorToInt((3 + Mathf.Sqrt((12 * chunkConfig.chunkNum) - 3)) / 6);
            int side = Mathf.CeilToInt((float)chunkConfig.chunkNum / depth) - (3 * depth) + 2;

            SetRotation(chunkView, chunkConfig, depth, side);
            SetBirdseyePosition(chunkView, chunkConfig, depth, side);
        }

        private void SetRotation(ChunkView chunkView, ChunkConfig chunkConfig, int depth, int side)
        {
            int depthStartChunkNum = (3 * depth * (depth - 1)) + 1;
            int depthEndChunkNum = 3 * depth * (depth + 1);

            if (chunkConfig.chunkNum == depthStartChunkNum || chunkConfig.chunkNum == depthEndChunkNum)
            {
                side = 5;
            }

            side = (side + 4) % 6;

            chunkView.Rotation *= Quaternion.AngleAxis(
                side * 60,
                chunkView.Up
            );
        }

        private void SetBirdseyePosition(ChunkView chunkView, ChunkConfig chunkConfig, int depth, int side)
        {
            int depthStartChunkNum = (3 * depth * (depth - 1)) + 1;
            int sideStartChunkNum = depthStartChunkNum + (depth * side);
            int sideChunkNum = chunkConfig.chunkNum - sideStartChunkNum;

            switch (side)
            {
                case 0:
                    chunkView.Position = Position(chunkConfig, chunkReference.Northeast, chunkReference.Northwest, sideChunkNum, depth);
                    return;
                case 1:
                    chunkView.Position = Position(chunkConfig, chunkReference.East, chunkReference.Northeast, sideChunkNum, depth);
                    return;
                case 2:
                    chunkView.Position = Position(chunkConfig, chunkReference.Southeast, chunkReference.East, sideChunkNum, depth);
                    return;
                case 3:
                    chunkView.Position = Position(chunkConfig, chunkReference.Southwest, chunkReference.Southeast, sideChunkNum, depth);
                    return;
                case 4:
                    chunkView.Position = Position(chunkConfig, chunkReference.West, chunkReference.Southwest, sideChunkNum, depth);
                    return;
                case 5:
                    chunkView.Position = Position(chunkConfig, chunkReference.Northwest, chunkReference.West, sideChunkNum, depth);
                    return;
            }

            Debug.LogError("Failed to compute spiral chunk position");
        }

        private Vector3 Position(ChunkConfig chunkConfig, Vector3 sideDirection, Vector3 depthDirection, int sideChunkNum, int depth)
        {
            return ((sideChunkNum + 1) * sideDirection) + ((depth - sideChunkNum - 1) * depthDirection) + Altitude(chunkConfig);
        }

        private Vector3 Altitude(ChunkConfig chunkConfig)
        {
            return 2 * Vector3.up * (chunkCount - chunkConfig.chunkNum);
        }
    }
}
