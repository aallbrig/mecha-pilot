using System.Collections.Generic;
using Combat;
using Combat.OnDiedBehavior;
using Gameplay.OutOfPlaySphereBehaviors;
using UnityEngine;

namespace Spawners
{
    public class BeginnerRepeatSpawner : RepeatSpawner
    {
        [Range(1.0f, 10.0f)] public float enemiesLimit = 1.0f;
        public float amountToAddToEnemyCountPerKill = 0.25f;
        public float beginnerTimeBuffer = 3f;
        public float bufferToDecreaseByPerKill = 0.2f;
        private readonly List<GameObject> alreadyListening = new();
        private float _currentBeginnerTimeBuffer;
        private int _currentEnemiesCount;
        private float _currentEnemiesLimit;

        private void OnEnable()
        {
            _currentBeginnerTimeBuffer = beginnerTimeBuffer;
            _currentEnemiesLimit = enemiesLimit;
            _currentEnemiesCount = 0;
        }
        protected override void OnSpawnerSpawned(GameObject spawnee)
        {
            base.OnSpawnerSpawned(spawnee);

            _currentEnemiesCount++;

            // quick fix: only bind listeners once
            if (!alreadyListening.Contains(spawnee))
            {
                if (spawnee.TryGetComponent<DeactivateWhenOutsidePlaySphere>(out var outsidePlaySphere))
                {
                    void OnDeactivated() => _currentEnemiesCount--;

                    outsidePlaySphere.Deactivated += OnDeactivated;
                }
                if (spawnee.TryGetComponent<DeactivateOnPlayerDeath>(out var onPlayerDied))
                {
                    void OnDeactivated() => _currentEnemiesCount--;

                    onPlayerDied.Deactivated += OnDeactivated;
                }
                if (spawnee.TryGetComponent<ICanDie>(out var ableToDie))
                {
                    void OnDied(GameObject deadGameObject)
                    {
                        _currentEnemiesCount--;
                        _currentEnemiesLimit += amountToAddToEnemyCountPerKill;
                        _currentBeginnerTimeBuffer -= bufferToDecreaseByPerKill;
                    }

                    ableToDie.Died += OnDied;
                }
                alreadyListening.Add(spawnee);
            }
        }
        private void OnSpawnedGameObjectDied(GameObject deadGameObject) {}
        protected override bool ShouldSpawn()
        {
            var calculatedLimit = (int)Mathf.Floor(_currentEnemiesLimit);
            var shouldSpawn = _currentEnemiesCount < calculatedLimit && base.ShouldSpawn();
            return shouldSpawn;
        }
        protected override float NewDelayTime()
        {
            var calculatedDelayTime = _currentBeginnerTimeBuffer > 0f
                ? Random.Range(minDelayTimeInSeconds + _currentBeginnerTimeBuffer,
                    maxDelayTimeInSeconds + _currentBeginnerTimeBuffer)
                : base.NewDelayTime();
            return calculatedDelayTime;
        }
    }
}
