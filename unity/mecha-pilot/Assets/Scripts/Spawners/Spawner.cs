using System;
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
        public GameObject prefabToSpawn;
        public SpawnType spawnType = SpawnType.Repeat;
        [Range(0f, 50f)] public float minDelayTimeInSeconds;
        [Range(0f, 50f)] public float maxDelayTimeInSeconds;
        private bool _active = true;
        private float _delayTime;
        private float _timeOfLastSpawn;
        private void Start()
        {
            _timeOfLastSpawn = Time.time - maxDelayTimeInSeconds;
            _delayTime = NewDelayTime();
        }
        private void Update()
        {
            if (_active && Time.time - _timeOfLastSpawn > _delayTime) Spawn();
        }
        private void OnValidate() => _active = prefabToSpawn != null;

        public event Action<GameObject> Spawned;

        private float NewDelayTime() => Random.Range(minDelayTimeInSeconds, maxDelayTimeInSeconds);
        [ContextMenu("Spawn")]
        private void Spawn()
        {
            if (!_active) return;

            var spawnedGameObject = Instantiate(prefabToSpawn);
            Spawned?.Invoke(spawnedGameObject);
            _timeOfLastSpawn = Time.time;
            _delayTime = NewDelayTime();
        }
    }
}
