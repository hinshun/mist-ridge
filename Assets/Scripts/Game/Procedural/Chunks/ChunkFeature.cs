using UnityEngine;

namespace MistRidge
{
    [CreateAssetMenu(menuName = "Chunks/Feature")]
    public class ChunkFeature : ScriptableObject
    {
        [SerializeField]
        private ChunkFeatureView chunkFeatureView;

        public ChunkFeatureView ChunkFeatureView
        {
            get
            {
                return chunkFeatureView;
            }
        }
    }
}
