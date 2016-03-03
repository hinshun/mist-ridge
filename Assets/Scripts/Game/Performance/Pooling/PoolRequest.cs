using UnityEngine;
using System;

namespace MistRidge
{
    [Serializable]
    public struct PoolRequest
    {
        public PoolInstanceView poolInstanceView;
        public int poolSize;
    }
}
