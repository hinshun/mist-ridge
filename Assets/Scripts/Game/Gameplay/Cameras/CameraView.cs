using UnityEngine;

namespace MistRidge
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoView
    {
        new private Camera camera;
        private AudioListener audioListener;

        private bool isActive;

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

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                /* Camera.enabled = value; */
                /* AudioListener.enabled = value; */
            }
        }

        private void Awake()
        {
            camera = GetComponent<Camera>();
            audioListener = GetComponent<AudioListener>();
        }
    }
}
