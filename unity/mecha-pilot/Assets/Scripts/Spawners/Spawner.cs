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
        protected virtual void OnSpawnerSpawned(GameObject spawnedGameObject) {}
        [ContextMenu("Spawn")]
        protected void Spawn()
        {
            var spawnedGameObject = GetGameObjectToSpawn();
            TimeOfLastSpawn = Time.time;
            // allow descendants to process spawned game objects in the same frame
            OnSpawnerSpawned(spawnedGameObject);
            Spawned?.Invoke(spawnedGameObject);
        }
        private GameObject GetGameObjectToSpawn()
        {
            var poolObject = spawneePool.GetPoolObject();
            return poolObject;
        }
    }
}
