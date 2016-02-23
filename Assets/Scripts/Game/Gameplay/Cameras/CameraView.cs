using UnityEngine;

namespace MistRidge
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoView
    {
        new private Camera camera;
        private AudioListener audioListener;

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        public AudioListener AudioListener
        {
            get
            {
                return audioListener;
            }
        }

        public float HorizontalTanFov
        {
            get
            {
                return Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2f) * camera.aspect;
            }
        }

        public float VerticalTanFov
        {
            get
            {
                return Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2f);;
            }
        }

        private void Awake()
        {
            camera = GetComponent<Camera>();
            audioListener = GetComponent<AudioListener>();
        }
    }
}
