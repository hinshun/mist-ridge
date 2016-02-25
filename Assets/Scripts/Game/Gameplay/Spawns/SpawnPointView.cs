using UnityEngine;

namespace MistRidge
{
    public class SpawnPointView : MonoView
    {
        [SerializeField]
        private Mesh spawnPointMesh;

        public void OnDrawGizmos()
        {
            if (spawnPointMesh != null)
            {
                DrawSpawnPointGizmos();
            }
        }

        private void DrawSpawnPointGizmos()
        {
            Gizmos.color = Color.cyan - new Color(0, 0, 0, 0.9f);

            Gizmos.DrawWireMesh(
                spawnPointMesh,
                Position + Up
            );
        }
    }
}
