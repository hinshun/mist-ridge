using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Checkpoint
    {
        public class Factory : Factory<Checkpoint>
        {
        }
    }
}
