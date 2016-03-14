using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Chunk Feature")]
    public class ChunkFeature : ScriptableObject
    {
        [SerializeField]
        private ChunkFeatureView chunkFeatureView;

        [SerializeField]
        private ChunkFeature chunkChainNext;

        [SerializeField]
        private int rarity;

        [SerializeField]
        private bool isUnique;

        [SerializeField]
        private bool skipCorners;

        public ChunkFeatureView ChunkFeatureView
        {
            get
            {
                return chunkFeatureView;
            }
        }

        public ChunkFeature ChunkChainNext
        {
            get
            {
                return chunkChainNext;
            }
        }

        public int Rarity
        {
            get
            {
                return rarity;
            }
        }

        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
        }

        public bool SkipCorners
        {
            get
            {
                return skipCorners;
            }
        }
    }
}
