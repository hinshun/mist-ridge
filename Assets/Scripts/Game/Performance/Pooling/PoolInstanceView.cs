using UnityEngine;
using System;

namespace MistRidge
{
    public class PoolInstanceView : MonoView
    {
        public virtual void OnPoolInstanceReuse()
        {
            // Do Nothing
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}
