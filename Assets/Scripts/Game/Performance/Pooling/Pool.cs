using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class Pool : IInitializable
    {
        private readonly ObjectPoolView objectPoolView;
        private readonly PoolRequest poolRequest;
        private readonly PoolView poolView;

        private Queue<PoolInstanceView> poolInstanceViews;

        public Pool(
                PoolView poolView,
                PoolRequest poolRequest,
                ObjectPoolView objectPoolView)
        {
            this.poolView = poolView;
            this.poolRequest = poolRequest;
            this.objectPoolView = objectPoolView;
        }

        public int Count
        {
            get
            {
                return poolInstanceViews.Count;
            }
        }

        public void Initialize()
        {
            GameObject prefab = poolRequest.poolInstanceView.gameObject;

            poolInstanceViews = new Queue<PoolInstanceView>();
            poolView.Name = prefab.name + " pool";
            poolView.Parent = objectPoolView.transform;

            for (int i = 0; i < poolRequest.poolSize; ++i)
            {
                PoolInstanceView poolInstanceView = GameObject.Instantiate(prefab).GetComponent<PoolInstanceView>();
                poolInstanceView.Parent = poolView.transform;
                poolInstanceView.gameObject.SetActive(false);

                Enqueue(poolInstanceView);
            }
        }

        public PoolInstanceView ReusePoolInstance(Vector3 position, Quaternion rotation)
        {
            PoolInstanceView poolInstanceView = Dequeue();

            poolInstanceView.Position = position;
            poolInstanceView.Rotation = rotation;
            poolInstanceView.gameObject.SetActive(true);
            poolInstanceView.OnPoolInstanceReuse();

            Enqueue(poolInstanceView);

            return poolInstanceView;
        }

        public void Clear()
        {
            foreach (PoolInstanceView poolInstanceView in poolInstanceViews)
            {
                poolInstanceView.Destroy();
            }
        }

        public void Enqueue(PoolInstanceView poolInstanceView)
        {
            poolInstanceViews.Enqueue(poolInstanceView);
        }

        public PoolInstanceView Dequeue()
        {
            return poolInstanceViews.Dequeue();
        }

        public class Factory : Factory<PoolRequest, Pool>
        {
        }
    }
}
