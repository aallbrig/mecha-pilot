using System;
using UnityEngine;

namespace Combat
{
    public interface ICanDie
    {
        public event Action<GameObject> Died;
    }
}
