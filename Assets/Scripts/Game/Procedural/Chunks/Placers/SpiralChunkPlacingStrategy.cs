using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class SpiralChunkPlacingStrategy : IChunkPlacingStrategy, IInitializable
    {
        private Vector3 topLeft;
        private Vector3 topRight;
        private Vector3 left;
        private Vector3 right;
        private Vector3 bottomLeft;
        private Vector3 bottomRight;

        public void Initialize()
        {
            topLeft = Unit(new Vector2(3f, -4f));
            topRight = Unit(new Vector2(-1.5f, -5f));
            left = Unit(new Vector2(4.5f, 1f));
            right = Unit(new Vector2(-4.5f, -1f));
            bottomLeft = Unit(new Vector2(1.5f, 5f));
            bottomRight = Unit(new Vector2(-3f, 4f));
        }

        public Vector3 Placement(ChunkConfig chunkConfig)
        {
            if (chunkConfig.chunkNum == 0)
            {
                return Vector3.zero;
            }

            int depth = Mathf.FloorToInt((3 + Mathf.Sqrt((12 * chunkConfig.chunkNum) - 3)) / 6);
            int depthStartChunkNum = (3 * depth * (depth - 1)) + 1;

            int side = Mathf.CeilToInt((float)chunkConfig.chunkNum / depth) - 3 * depth + 2;
            int sideStartChunkNum = depthStartChunkNum + (depth * side);
            int sideChunkNum = chunkConfig.chunkNum - sideStartChunkNum;

            switch (side)
            {
                case 0:
                    return Position(topLeft, topRight, sideChunkNum, depth, chunkConfig.chunkNum);
                case 1:
                    return Position(left, topLeft, sideChunkNum, depth, chunkConfig.chunkNum);
                case 2:
                    return Position(bottomLeft, left, sideChunkNum, depth, chunkConfig.chunkNum);
                case 3:
                    return Position(bottomRight, bottomLeft, sideChunkNum, depth, chunkConfig.chunkNum);
                case 4:
                    return Position(right, bottomRight, sideChunkNum, depth, chunkConfig.chunkNum);
                case 5:
                    return Position(topRight, right, sideChunkNum, depth, chunkConfig.chunkNum);
            }

            Debug.LogError("Failed to compute spiral chunk position");
            return Vector3.zero;
        }

        private Vector3 Unit(Vector2 direction)
        {
            float xScale = 4f;
            float yScale = 4f;

            return new Vector3(
                direction.x * xScale,
                0,
                direction.y * yScale * Mathf.Sqrt(3) / 2
            );
        }

        private Vector3 Position(Vector3 sideDirection, Vector3 depthDirection, int sideChunkNum, int depth, int chunkNum)
        {
            return ((sideChunkNum + 1) * sideDirection) + ((depth - sideChunkNum - 1) * depthDirection) + (Vector3.down * chunkNum);
        }
    }
}

