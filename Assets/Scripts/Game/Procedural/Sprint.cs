using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class Sprint
    {
        public class Factory : Factory<Sprint>
        {
        }
    }
}