using System;
using UnityEngine;

namespace Combat
{
    public interface ICanDie
    {
        public float TimeOfDeath { get; }

        public event Action<GameObject> Died;
    }
}
