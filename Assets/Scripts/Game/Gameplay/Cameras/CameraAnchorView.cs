using UnityEngine;

namespace MistRidge
{
    public class CameraAnchorView : MonoView
    {
        [SerializeField]
        private ParticleSystem snow;

        public ParticleSystem Snow
        {
            get
            {
                return snow;
            }
        }
    }
}
