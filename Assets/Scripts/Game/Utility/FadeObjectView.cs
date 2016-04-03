using UnityEngine;
using UnityEngine.UI;

namespace MistRidge
{
    public class FadeObjectView : MonoView
    {
        private Image image;

        public void UpdateColor(Color color)
        {
            image.color = color;
        }

        private void Awake()
        {
            image = GetComponent<Image>();
        }
    }
}
