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

        public void Remove()
        {
            gameObject.SetActive(false);
        }
    }
}
