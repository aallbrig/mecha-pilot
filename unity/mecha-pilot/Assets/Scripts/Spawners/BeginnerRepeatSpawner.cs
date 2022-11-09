using UnityEngine;

namespace Spawners
{
    public class RepeatSpawner : Spawner
    {
        [Range(0f, 50f)] public float minDelayTimeInSeconds;
        [Range(0f, 50f)] public float maxDelayTimeInSeconds;
        private float _delayTime;
        private void Start()
        {
            TimeOfLastSpawn = Time.time - maxDelayTimeInSeconds;
            _delayTime = NewDelayTime();
            Spawned += OnSpawnerSpawned;
        }
        protected override bool ShouldSpawn() => Time.time - TimeOfLastSpawn > _delayTime;
        private void OnSpawnerSpawned(GameObject obj) => _delayTime = NewDelayTime();
        private float NewDelayTime() => Random.Range(minDelayTimeInSeconds, maxDelayTimeInSeconds);
    }
}
