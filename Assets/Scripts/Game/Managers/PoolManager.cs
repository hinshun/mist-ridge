using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class PoolManager : IInitializable
    {
        private readonly Settings settings;
        private readonly ObjectPoolView objectPoolView;
        private readonly Pool.Factory poolFactory;

        private Dictionary<int, Pool> pools;

        public PoolManager(
                Settings settings,
                ObjectPoolView objectPoolView,
                Pool.Factory poolFactory)
        {
            this.settings = settings;
            this.objectPoolView = objectPoolView;
            this.poolFactory = poolFactory;
        }

        public void Initialize()
        {
            pools = new Dictionary<int, Pool>();
            CreatePools();
        }

        public void ReusePoolInstance(PoolInstanceView poolInstanceView, Vector3 position, Quaternion rotation)
        {
            int poolKey = poolInstanceView.gameObject.GetInstanceID();

            if (pools.ContainsKey(poolKey))
            {
                pools[poolKey].ReusePoolInstance(position, rotation);
            }
        }

        private void CreatePools()
        {
            foreach (PoolRequest poolRequest in settings.poolRequests)
            {
                int poolKey = poolRequest.poolInstanceView.gameObject.GetInstanceID();

                if (!pools.ContainsKey(poolKey))
                {
                    pools[poolKey] = poolFactory.Create(poolRequest);
                    pools[poolKey].Initialize();
                }
                else
                {
                    Pool pool = poolFactory.Create(poolRequest);
                    while (pool.Count > 0)
                    {
                        pools[poolKey].Enqueue(pool.Dequeue());
                    }
                }
            }
        }

        [Serializable]
        public class Settings
        {
            public GameObject objectPoolPrefab;
            public GameObject poolPrefab;
            public List<PoolRequest> poolRequests;
        }
    }
}
