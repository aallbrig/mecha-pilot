using System;
using Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public enum SpawnType
    {
        Repeat
    }

    public interface ISpawner
    {
        public event Action<GameObject> Spawned;
    }

    public class Spawner : MonoBehaviour, ISpawner
    {
        public SpawnType spawnType = SpawnType.Repeat;
        public ObjectPool spawneePool;
        [Range(0f, 50f)] public float minDelayTimeInSeconds;
        [Range(0f, 50f)] public float maxDelayTimeInSeconds;
        private readonly bool _active = true;
        private float _delayTime;
        private float _timeOfLastSpawn;
        private void Start()
        {
            _timeOfLastSpawn = Time.time - maxDelayTimeInSeconds;
            _delayTime = NewDelayTime();
            spawneePool ??= GetComponent<ObjectPool>();
        }
        private void Update()
        {
            if (_active && Time.time - _timeOfLastSpawn > _delayTime) Spawn();
        }

        public event Action<GameObject> Spawned;

        private float NewDelayTime() => Random.Range(minDelayTimeInSeconds, maxDelayTimeInSeconds);
        [ContextMenu("Spawn")]
        private void Spawn()
        {
            if (!_active) return;

            var spawnedGameObject = GetGameObjectToSpawn();
            Spawned?.Invoke(spawnedGameObject);
            _timeOfLastSpawn = Time.time;
            _delayTime = NewDelayTime();
        }
        private GameObject GetGameObjectToSpawn()
        {
            var poolObject = spawneePool.GetPoolObject();
            poolObject.SetActive(true);
            return poolObject;
        }
    }
}
