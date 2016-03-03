using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class SpiralChunkPlacingStrategy : IChunkPlacingStrategy
    {
        private readonly ChunkReference chunkReference;

        public SpiralChunkPlacingStrategy(ChunkReference chunkReference)
        {
            this.chunkReference = chunkReference;
        }

        public void Place(ChunkView chunkView, ChunkRequest chunkRequest)
        {
            if (chunkRequest.chunkNum == 0)
            {
                chunkView.Rotation *= Quaternion.AngleAxis(
                    120,
                    chunkView.Up
                );
                chunkView.Position = Vector3.zero + Altitude(chunkRequest);
                return;
            }

            int depth = Mathf.FloorToInt((3 + Mathf.Sqrt((12 * chunkRequest.chunkNum) - 3)) / 6);
            int side = Mathf.CeilToInt((float)chunkRequest.chunkNum / depth) - (3 * depth) + 2;

			chunkView.Rotation *= BirdseyeRotation(chunkView, chunkRequest, depth, side);
			chunkView.Position = BirdseyePosition(chunkView, chunkRequest, depth, side);
        }

        private Quaternion BirdseyeRotation(ChunkView chunkView, ChunkRequest chunkRequest, int depth, int side)
        {
            int depthStartChunkNum = (3 * depth * (depth - 1)) + 1;
            int depthEndChunkNum = 3 * depth * (depth + 1);

            if (chunkRequest.chunkNum == depthStartChunkNum || chunkRequest.chunkNum == depthEndChunkNum)
            {
                side = 5;
            }

            side = (side + 4) % 6;

            return Quaternion.AngleAxis(
                side * 60,
                chunkView.Up
            );
        }

        private Vector3 BirdseyePosition(ChunkView chunkView, ChunkRequest chunkRequest, int depth, int side)
        {
            int depthStartChunkNum = (3 * depth * (depth - 1)) + 1;
            int sideStartChunkNum = depthStartChunkNum + (depth * side);
            int sideChunkNum = chunkRequest.chunkNum - sideStartChunkNum;

            switch (side)
            {
                case 0:
                    return Position(chunkRequest, chunkReference.Northeast, chunkReference.Northwest, sideChunkNum, depth);
                case 1:
                    return Position(chunkRequest, chunkReference.East, chunkReference.Northeast, sideChunkNum, depth);
                case 2:
                    return Position(chunkRequest, chunkReference.Southeast, chunkReference.East, sideChunkNum, depth);
                case 3:
                    return Position(chunkRequest, chunkReference.Southwest, chunkReference.Southeast, sideChunkNum, depth);
                case 4:
                    return Position(chunkRequest, chunkReference.West, chunkReference.Southwest, sideChunkNum, depth);
                case 5:
                    return Position(chunkRequest, chunkReference.Northwest, chunkReference.West, sideChunkNum, depth);
            }

            Debug.LogError("Failed to compute spiral chunk position");
			return Vector3.zero;
        }

        private Vector3 Position(ChunkRequest chunkRequest, Vector3 sideDirection, Vector3 depthDirection, int sideChunkNum, int depth)
        {
            return ((sideChunkNum + 1) * sideDirection) + ((depth - sideChunkNum - 1) * depthDirection) + Altitude(chunkRequest);
        }

        private Vector3 Altitude(ChunkRequest chunkRequest)
        {
            return 2 * Vector3.up * (chunkRequest.chunkCount - chunkRequest.chunkNum) + (20 * Vector3.up);
        }
    }
}
