using UnityEngine;
using System;

namespace MistRidge
{
    public class BoundsView : MonoView
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("entered " + other.gameObject.name);
            ChunkFeatureView chunkFeatureView = other.GetComponent<ChunkFeatureView>();

            if (chunkFeatureView != null)
            {
                foreach(MeshRenderer meshRenderer in chunkFeatureView.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.enabled = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("exited" + other.gameObject.name);
            ChunkFeatureView chunkFeatureView = other.GetComponent<ChunkFeatureView>();

            if (chunkFeatureView != null)
            {
                foreach(MeshRenderer meshRenderer in chunkFeatureView.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }
}
