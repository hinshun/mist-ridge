using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Spawn
    {
        private readonly SpawnView spawnView;

        public Spawn(SpawnView spawnView)
        {
            this.spawnView = spawnView;
        }

        public Vector3 Position
        {
            get
            {
                return spawnView.Position;
            }
            set
            {
                spawnView.Position = value;
            }
        }

        public Vector3 SpawnPoint(int index)
        {
            return spawnView.SpawnPointViews[index].Position;
        }

        public class Factory : Factory<Spawn>
        {
        }
    }
}
