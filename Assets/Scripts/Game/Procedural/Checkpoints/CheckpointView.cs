using UnityEngine;

namespace MistRidge
{
    public class CheckpointView : MonoView
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerView playerView = other.GetComponent<PlayerView>();
                playerView.OnCheckpoint(this);
            }
        }
    }
}
