using UnityEngine;
using System;

namespace MistRidge
{
    public class PlayerContainerView : MonoView
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
