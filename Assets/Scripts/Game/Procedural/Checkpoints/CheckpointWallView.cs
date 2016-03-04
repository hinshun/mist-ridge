using UnityEngine;

namespace MistRidge
{
    public class CheckpointWallView : MonoView
    {
        public void Open()
        {
            transform.position += Vector3.down * 10;
        }
    }
}
