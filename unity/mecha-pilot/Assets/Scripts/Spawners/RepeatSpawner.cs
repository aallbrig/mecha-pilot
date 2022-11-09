using UnityEngine;

namespace Spawners
{
    public class RepeatSpawner : Spawner
    {
        [Range(0f, 50f)] public float minDelayTimeInSeconds;
        [Range(0f, 50f)] public float maxDelayTimeInSeconds;
        private float _delayTime;
        private void OnEnable()
        {
            TimeOfLastSpawn = Time.time - maxDelayTimeInSeconds;
            _delayTime = NewDelayTime();
        }
        protected override bool ShouldSpawn() => Time.time - TimeOfLastSpawn > _delayTime;
        protected override void OnSpawnerSpawned(GameObject obj) => _delayTime = NewDelayTime();
        protected virtual float NewDelayTime() => Random.Range(minDelayTimeInSeconds, maxDelayTimeInSeconds);
    }
}
