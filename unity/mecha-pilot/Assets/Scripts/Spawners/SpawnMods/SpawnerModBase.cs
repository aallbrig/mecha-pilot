using System;
using UnityEngine;

namespace Spawners.SpawnMods
{
    public class SpawnerModBase : MonoBehaviour
    {
        protected Spawner Spawner;
        private void Awake()
        {
            Spawner ??= GetComponent<Spawner>();
            Spawner ??= FindObjectOfType<Spawner>();
            if (Spawner == null) throw new NullReferenceException("Unable to locate spawner");
        }
    }
}
