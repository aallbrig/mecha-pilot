using System;
using Gameplay;
using UnityEngine;

namespace Spawners
{
    public interface ISpawner
    {
        public event Action<GameObject> Spawned;
    }

    public abstract class Spawner : MonoBehaviour, ISpawner
    {
        public ObjectPool spawneePool;
        protected float TimeOfLastSpawn;
        private void Start() => spawneePool ??= GetComponent<ObjectPool>();
        private void Update()
        {
            if (ShouldSpawn()) Spawn();
        }

        public event Action<GameObject> Spawned;

        protected abstract bool ShouldSpawn();
        [ContextMenu("Spawn")]
        protected void Spawn()
        {
            var spawnedGameObject = GetGameObjectToSpawn();
            TimeOfLastSpawn = Time.time;
            Spawned?.Invoke(spawnedGameObject);
        }
        private GameObject GetGameObjectToSpawn()
        {
            var poolObject = spawneePool.GetPoolObject();
            return poolObject;
        }
    }
}
